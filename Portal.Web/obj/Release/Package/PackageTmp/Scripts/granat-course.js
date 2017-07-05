


function setWatchedModule(moduleId) {
    $.ajax({ url: '/Ajax/_ajax_SetCurrentModuleId?id=' + moduleId, async: false });
}
function toggleFullScreen() {
    if (!document.fullscreenElement &&    // alternative standard method
        !document.mozFullScreenElement && !document.webkitFullscreenElement) {  // current working methods
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
            document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    } else {
        if (document.cancelFullScreen) {
            document.cancelFullScreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
            document.webkitCancelFullScreen();
        }
    }
}

$(function () {
   // var courseModules = getCourseList();
    var courseModules = movieList;



    var itemWidth = (100.0 / courseModules.length) - 0.4;
    var activeItem = -1;
    var lastComplete = lastSeen;

    if (requestedModule > -1 && requestedModule < lastComplete) {
        activeItem = requestedModule;
    }
    else if (lastComplete > 0) {
        activeItem = lastComplete-1;
    } else {
        activeItem = 0;
    }


    
    if (lastComplete == 13) {
        $("#examIcon").wrap($('<a>', {
            href: '/Exam/'
        }));
    }
            
    var activeQuestion = 0;
    var froogaloop;

    // Tilbakestiller spiller. Url settes til activeItems video
    function resetPlayer() {
        // Setter fremdriftstext. Eks: 4 / 12
        $(".course-progress-text").text(activeItem + 1 + ' / ' + courseModules.length);
        // Setter URL til spiller
        $("#vimeoplayer")[0].src = courseModules[activeItem].VideoUrl + '?api=1&player_id=vimeoplayer';
        $(".course-progress-item").removeClass('active');
        $($(".course-progress-item")[activeItem]).addClass('active');
        $('#questionTrigger').addClass('disabled');
    }

    function enableNextQuestion(enable) {
        if (enable)
            $("#nextQuestion").removeClass('disabled');
        else
            $("#nextQuestion").addClass('disabled');
    }

    function playerNext() {
        setWatchedModule(activeItem);

        if (activeItem + 1 >= courseModules.length) {
            window.location.replace("/Exam/Start");
            return false;
        } else {
            activeItem += 1;
            // Logge til server at vi er gått til neste modul

            activeQuestion = 0;
           // return true;
             resetPlayer();
        }
    }

    function playerPrevious() {

        if (activeItem == 0)
            return;

            activeItem -= 1;
            
            activeQuestion = 0;
            // return true;
            resetPlayer();
    }

    function setPlayerIndex(index) {
        // Flytter til en bestemt film basert på indeks
        activeItem = index;
        activeQuestion = 0;
        resetPlayer();
    }

    // Viser spørsmålsvindu
    function setupQuestion(module) {
        if (module.Questions.length <= 0) {
            // Ingen spørsmål. Lar brukeren få gå direkte videre til neste film.
            
            enableNextQuestion(true);
            //$("#nextQuestion").click();
            $("#questionModal").modal('hide');
            
        } else {


            var alternatives = module.Questions[activeQuestion].Alternatives;
            $("#questionModal .alternatives").empty();
            $("#questionModal .response").empty().append('<i class="fa"> </i>');

            enableNextQuestion(false);


            for (var i = 0; i < alternatives.length; i++) {
                var button = $('<button id="alternative_' + i + '" class="btn-transparent btn-alternative left"><img class="thumbnail img-responsive img-alternative" src="' + alternatives[i].ImgUrl + '" /></button>');
                button.click(function() {
                    var button = $(this);
                    var i = button.attr('id').split('_')[1];

                    button.parent().find("button i.fa").fadeOut(function() {
                        $(this).remove();
                    });

                    if (alternatives[i].IsCorrect) {
                        button.append('<i class="fa fa-check"></i>');
                        $("#questionModal .response").empty().append('<i class="fa fa-smile-o"></i>');
                        enableNextQuestion(true);
                        // TODO: Automatisk gå til neste spørsmål etter X antall sekunder
                    } else {

                        //button.vibrate();
                        button.append('<i class="fa fa-times"></i>');

                        $("#questionModal .response").empty().append('<i class="fa fa-frown-o"></i>');

                        enableNextQuestion(false);
                    }
                });

                $("#questionModal .alternatives").append(button);
            }
        }

    }

    $("#player-prev").click(function() {
        playerPrevious();
    });

    $("#player-next").click(function () {
        
        if (activeItem < lastComplete -1)
            playerNext();
    });

    function addPlayerEventHandlers() {

        // First question
        $("#questionTrigger").click(function () {
            $f($("#vimeoplayer")[0]).api('pause');

                setupQuestion(courseModules[activeItem]);
            
        });

        // Event som fyres av knappen som går videre til neste spørsmål
        $("#nextQuestion").click(function () {
            // Sjekker om vi er kommet til siste spørsmål
            if (activeQuestion + 1 >= courseModules[activeItem].Questions.length) {
                // Vi er ferdig med siste spørsmål. Går til neste modul
                courseModules[activeItem].completed = true;
                $($(".course-progress-item")[activeItem]).addClass('complete');
                $("#questionModal").modal('hide');
             
                var reset = playerNext();

                if (reset == false) {
                    resetPlayer();
                }
            }
            else {
                // Next question
                activeQuestion++;
                setupQuestion(courseModules[activeItem]);
            }


        });
    }

    function setupVideoPlayer() {
        var player = $f($("#vimeoplayer")[0]);
        player.addEvent('ready', onVideoPlayerReady);
    }

    function onVideoPlayerReady(player_id) {
        froogaloop = $f(player_id);

        froogaloop.addEvent('play', function () {
         //   console.log('play');
        });
        froogaloop.addEvent('finish', function () {
            toggleFullScreen();
         //   alert('run video');
            $('#questionTrigger').removeClass('disabled');
            // tord - questionTrigger i ikonet i bunn.. viser spørsmålene
            // TODO: Det må her sjekkes om vi har spørsmål til modulen eller ikke. I første versjon hardcodes det
            // Første modul har ingen spørsmål
            if (activeItem != 0) {
                $('#questionTrigger').click();
            } else {
                // Setter fargen på fremdriftsviseren til å være ferdig.
                $($(".course-progress-item")[activeItem]).addClass('complete');
                playerNext();
            }
        });

        froogaloop.api('play');
    }

    function setUpPlayer() {
        

        for (var i = 0; i < courseModules.length; i++) {
            var isActive = false;
            var thisItem = courseModules[i];
            var index = i;


            if (thisItem.completed)
                lastComplete = i;
            else {
                isActive = (lastComplete == i);
                if (i < lastComplete) {
                    thisItem.completed = true;
                }
            }

            // Tooltip ved mouseover
            var item = $('<div rel="popover" data-trigger="hover" class="course-progress-item' + (thisItem.completed ? ' complete' : '') + (isActive ? ' active' : '') + '" style="width: ' + itemWidth + '%;"></div>');
            item.popover({
                placement: 'bottom',
                html: true,
                content: '<img src="' + thisItem.ThumbUrl + '" />',
            });
            item.click(function () {
                var clickedItem = $(this);
                var clickedIndex = clickedItem.index();

                if (clickedItem.hasClass('complete') || clickedItem.prev().hasClass('complete'))
                    setPlayerIndex(clickedIndex);
            });

            $(".course-progress-bar").append(item);
        }

        //activeItem = (activeItem> courseModules.length ? 0 : lastComplete);
        // Setter teksten for kursfremdrift
        $(".course-progress-text").text(activeItem + 1 + ' / ' + courseModules.length);
        // Setter videoens url
        $("#vimeoplayer")[0].src = courseModules[activeItem].VideoUrl + '?api=1&player_id=vimeoplayer';
    }


    setUpPlayer();
    setupVideoPlayer();
    addPlayerEventHandlers();

});
