(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'courseDataSvc';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('app').factory(serviceId, ['$http', CourseDataSvc]);
  
    function CourseDataSvc($http) {
    
        // Define the functions and properties to reveal.
        var service = {
            getData: getData,
            getQuestionsFlattened: getQuestionsFlattened(),
            getCourse: getCourse(),
            answerQuestion: answerQuestion(),
        };

        return service;

        // http://vimeo.com/89798070

        function getData() {
            return {
                course: {
                    name: 'HMS',
                    videoUrl: '//player.vimeo.com/video/89798070',
                    thumbUrl: '//b.vimeocdn.com/ts/442/776/442776569_100.jpg',
                    modules: [
                        {
                            videoUrl: '//player.vimeo.com/video/89925922',
                            thumbUrl: '//b.vimeocdn.com/ts/445/770/89925922_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        }
                        ,
                        {
                            // 2
                            videoUrl: '//player.vimeo.com/video/89798070',
                            thumbUrl: '//b.vimeocdn.com/ts/442/776/89798070_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        },
                        {
                            // 3
                            videoUrl: '//player.vimeo.com/video/89796952',
                            thumbUrl: '//b.vimeocdn.com/ts/445/770/89796952_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        },
                        { // 4
                            videoUrl: '//player.vimeo.com/video/89832552',
                            thumbUrl: '//b.vimeocdn.com/ts/442/776/89832552_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        },
                        {
                            // 5
                            videoUrl: '//player.vimeo.com/video/89799831',
                            thumbUrl: '//b.vimeocdn.com/ts/445/770/89799831_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        },
                        { // 6
                            videoUrl: '//player.vimeo.com/video/89800311',
                            thumbUrl: '//b.vimeocdn.com/ts/442/776/89800311_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        },
                        {
                            // 7
                            videoUrl: '//player.vimeo.com/video/89796853',
                            thumbUrl: '//b.vimeocdn.com/ts/445/770/89796853_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        },
                        {
                            // 8
                            videoUrl: '//player.vimeo.com/video/89901610',
                            thumbUrl: '//b.vimeocdn.com/ts/442/776/89901610_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        },
                        { // 9
                            videoUrl: '//player.vimeo.com/video/89797122',
                            thumbUrl: '//b.vimeocdn.com/ts/445/770/89797122_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        },
                        { // 10
                            videoUrl: '//player.vimeo.com/video/89797007',
                            thumbUrl: '//b.vimeocdn.com/ts/442/776/89797007_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        },
                        { // 11
                            videoUrl: '//player.vimeo.com/video/89796758',
                            thumbUrl: '//b.vimeocdn.com/ts/445/770/89796758_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        },
                        { // 12
                            videoUrl: '//player.vimeo.com/video/89796642',
                            thumbUrl: '//b.vimeocdn.com/ts/445/770/89796642_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        },
                        { // 13
                            videoUrl: '//player.vimeo.com/video/89795788',
                            thumbUrl: '//b.vimeocdn.com/ts/445/770/89795788_100.jpg',
                            questions: [
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q1a1.jpg',
                                            isCorrect: true,
                                        },
                                        {
                                            imgUrl: '/Content/img/q1a2.jpg',
                                            isCorrect: false,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q2a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q2a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                                {
                                    completed: false,
                                    alternatives: [
                                        {
                                            imgUrl: '/Content/img/q3a1.jpg',
                                            isCorrect: false,
                                        },
                                        {
                                            imgUrl: '/Content/img/q3a2.jpg',
                                            isCorrect: true,
                                        },
                                    ],
                                },
                            ],
                            completed: false,
                        }
                    ]
                }
            };
        }

        //#region Internal Methods        

        function getCourse() {
         
            var result;
            $.ajax({
                //The URL to process the request
                'url': '/Exam/GetCourse',
                //The type of request, also known as the "method" in HTML forms
                //Can be 'GET' or 'POST'
                'type': 'GET',
                'async': false,
                'success': function (data) {
                    
                    result = data;
                 }

            });
            
            return result;
        }

        function answerQuestion(questionId, success) {

            $.ajax({ url: '/Exam/AnswerQuestion?questionId=' + questionId + '&success=' + success, async : false });

        }

        
        function getQuestionsFlattened() {

            //       var deferred = $q.deferre
            var flat = [];
            $.ajax({
                //The URL to process the request
                'url': '/Exam/GetQuestions',
                //The type of request, also known as the "method" in HTML forms
                //Can be 'GET' or 'POST'
                'type': 'GET',
                'async': false,
                //Any post-data/get-data parameters
                //This is optional
                //        'data' : {
                //            'paramater1' : 'value'
                //          'parameter2' : 'another value'
                //},
                //The response from the server
                'success': function(data) {
                    for (var i = 0; i < data.length; i++) {
                        flat.push(data[i]);
                    }
                }

            });
         
            return flat;
      }

        function getQuestionsFlattened2() {
            var flat = [];
            var data = getData();
            for (var i = 0; i < data.course.modules.length; i++) {
                for (var j = 0; j < data.course.modules[i].questions.length; j++) {
                    flat.push(data.course.modules[i].questions[j]);
                }
            }
          
            return flat;
        }

        //#endregion
    }
})();