alert('ok');
function GetModuleStatistics(courseId) {

    // Registrerer svar... henter neste spørsmål dersom flere gjennstår
    var result;
    $.ajax({
        url: '/Exam/_ajax_GetCourseModuleStatistics',
        data: { id: courseId, d: new Date().getTime() },
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (data) {
            if (data == "-1") {
                // Ferdig med siste spørsmål.. what to do?
                alert('done');
            }
            $('#choise1').attr("src", data.ImageUrl1);
            $('#choise2').attr("src", data.ImageUrl2);

        },
        error: function (req, status, error) {
            alert('Klarte ikke å hente data fra server');
        }

    });

    return result;


}