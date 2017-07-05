(function() {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    //angular.module('app').directive('vimeoPlayer', ['$window', '$rootScope', vimeoPlayer]);
    
    //function vimeoPlayer($window, $rootScope) {
    //    // Usage:
    //    // 
    //    // Creates:
    //    // 
    // // TORD: Link parameter attrs is undefined
    //    var directive = {
    //        link: link,
    //        restrict: 'EA',
    //        replace: true,
    //        template: '<iframe id="vimeoplayer" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>',
    //        scope: {
    //        },
    //    };
    //    return directive;

    //    function link(scope, element, attrs) {
          
    //        element.attr('src', attrs.videoUrl + '?api=1&player_id=vimeoplayer');

    //        attrs.$observe('videoUrl', function (val) {
    //            element.attr('src', attrs.videoUrl + '?api=1&player_id=vimeoplayer');
    //        });

    //        scope.$on('vimeo-play', function () {
    //            post('play');
    //        });

    //        scope.$on('vimeo-pause', function () {
    //            post('pause');
    //        });

    //        scope.$on('vimeo-unload', function () {
    //            post('unload');
    //        });

    //        if ($window.addEventListener) {
    //            window.addEventListener('message', onMessageReceived, false);
    //        }
    //        else {
    //            $window.attachEvent('onmessage', onMessageReceived, false);
    //        }

    //        function onMessageReceived(e) {
    //            var data = JSON.parse(e.data);
    //            switch (data.event) {
    //                case 'ready':
    //                    onReady();

    //                    $rootScope.$broadcast("vimeo-readyEvent");
    //                    break;

    //                case 'playProgress':
    //                    $rootScope.$broadcast("vimeo-playProgressEvent", data.data);
    //                    break;

    //                case 'play':
    //                    $rootScope.$broadcast("vimeo-playEvent");
    //                    break;

    //                case 'pause':
    //                    $rootScope.$broadcast("vimeo-pauseEvent");
    //                    break;

    //                case 'finish':
    //                    $rootScope.$broadcast("vimeo-finishEvent");
    //                    break;
    //            }
    //        }

    //        function post(action, value) {
    //            var url = $window.location.protocol + element.attr('src').split('?')[0];
    //            var data = { method: action };

    //            if (value) {
    //                data.value = value;
    //            }
    //            element[0].contentWindow.postMessage(JSON.stringify(data), url);
    //        }

    //        function onReady() {
    //            console.log('ready');

    //            post('addEventListener', 'play');
    //            post('addEventListener', 'pause');
    //            post('addEventListener', 'finish');
    //            post('addEventListener', 'playProgress');
    //        }
    //    }
    //}

})();