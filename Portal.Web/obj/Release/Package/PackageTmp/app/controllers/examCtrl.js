(function () {
    'use strict';

    //// Controller name is handy for logging
    //var controllerId = 'examCtrl';

    //// Define the controller on the module.
    //// Inject the dependencies. 
    //// Point to the controller definition function.
    //angular.module('app').controller(controllerId,
    //    ['$scope', 'courseDataSvc', examCtrl]);

    //function examCtrl($scope, courseDataSvc) {
    //    // Bindable properties and functions are placed on vm.
    //    $scope.title = 'examCtrl';
        
    //    // Skiftet om på de to under
    //    $scope.course = courseDataSvc.getData().course;
    //    //$scope.course = courseDataSvc.getCourse();

        
    //    $scope.questions = courseDataSvc.getQuestionsFlattened;
      
    //    alert($scope.questions);

    //    $scope.questionItemWidth = (100.0 / $scope.questions.length) - 0.4;
    //    $scope.currentQuestionIndex = getNextQuestionIndex();
    //    $scope.currentQuestion = $scope.questions[$scope.currentQuestionIndex];
    //    $scope.submitAnswer = submitAnswer;




    //    //#region Internal Methods        
    //    function submitAnswer(alternative) {
    //        if (!$scope.currentQuestion.completed) {
                
    //            // Sender resultatet til serveren
    //            var result = false;

    //            if ($scope.currentQuestion.alternatives[0].isCorrect && alternative == 0)
    //            {
    //                result = true;
    //            } else if ($scope.currentQuestion.alternatives[1].isCorrect && alternative == 1)
    //            {
    //                result = true;
    //            }

    //            courseDataSvc.answerQuestion($scope.currentQuestion.questionId, result);

    //            $scope.currentQuestion.completed = true;

    //            if ($scope.currentQuestionIndex < $scope.questions.length - 1)
    //                $scope.currentQuestion = $scope.questions[++$scope.currentQuestionIndex];
    //        }
    //        else
    //        {
    //            // TODO: Siste spørsmål besvart.. What to do?
    //        }
    //    }

    //    function getNextQuestionIndex() {
    //        for (var i = 0; i < $scope.questions.length; i++) {
    //            if (!$scope.questions[i].completed)
    //                return i;
    //        }
    //        return -1;
    //    }


    //    //#endregion
    //}
})();
