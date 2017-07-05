(function () {
    'use strict';

    //var controllerId = 'coursePlayerCtrl';
    //angular.module('app').controller(controllerId,
    //    ['$scope', 'courseDataSvc', 'vimeoPlayerSvc', coursePlayerCtrl]);

    //function coursePlayerCtrl($scope, courseDataSvc, vimeoPlayerSvc) {
        
    //    $scope.title = 'coursePlayerCtrl';
    //    $scope.course = courseDataSvc.getData().course;
    //    $scope.moduleItemWidth = (100.0 / $scope.course.modules.length) - 0.4;
    //    $scope.lastCompletedModuleIndex = getLastCompletedModuleIndex();
    //    $scope.activeModuleIndex = $scope.lastCompletedModuleIndex + 1;
    //    $scope.activeModule = $scope.course.modules[$scope.activeModuleIndex];
    //    $scope.activeQuestionIndex = 0;
    //    $scope.activeQuestion = $scope.activeModule.questions[$scope.activeQuestionIndex];
    //    $scope.isActive = isActive;
    //    $scope.questionsEnabled = false;
    //    $scope.enableNextQuestion = false;

    //    $scope.moduleItemClick = moduleItemClick;
    //    $scope.submitAnswer = submitAnswer;
    //    $scope.nextQuestion = nextQuestion;
    //    $scope.nextModuleItem = nextModuleItem;

    //    $scope.$on('vimeo-readyEvent', onReady);
    //    $scope.$on('vimeo-playEvent', onPlay);
    //    $scope.$on('vimeo-pauseEvent', onPause);
    //    $scope.$on('vimeo-finishEvent', onFinish);


    //    function submitAnswer(alternativeIndex) {
    //        if ($scope.activeQuestion.alternatives[alternativeIndex].isCorrect)
    //            $scope.enableNextQuestion = true;
    //        else
    //            $scope.enableNextQuestion = false;

    //        //if ($scope.activeQuestionIndex < $scope.activeModule.questions.length - 1) {
    //        //    $scope.activeQuestion = $scope.activeModule.questions[++$scope.activeQuestionIndex];
    //        //    $scope.enableNextQuestion = false;
    //        //}
    //    }

    //    function nextQuestion() {
    //        if (!$scope.enableNextQuestion)
    //            return;

    //        if ($scope.activeQuestionIndex < $scope.activeModule.questions.length - 1) {
    //            $scope.activeQuestion = $scope.activeModule.questions[++$scope.activeQuestionIndex];
    //        }
    //        else {
    //            $("#questionModal").modal('hide');
    //            $scope.activeModuleIndex += 1;
    //            $scope.activeModule = $scope.course.modules[$scope.activeModuleIndex];
    //            $scope.activeQuestionIndex = 0;
    //            $scope.activeQuestion = $scope.activeModule.questions[$scope.activeQuestionIndex];
    //        }
    //        $scope.enableNextQuestion = false;
    //    }


    //    //#region Internal Methods        
    //    function isActive(module) {
    //        if ($scope.course.modules.indexOf(module) == $scope.activeModuleIndex)
    //            return true;
    //        return false;
    //    }

    //    function moduleItemClick(module) {
    //        var moduleIndex = $scope.course.modules.indexOf(module);
    //        //if (moduleIndex <= $scope.lastCompletedModuleIndex) {
    //            $scope.activeModuleIndex = moduleIndex;
    //            $scope.activeModule = $scope.course.modules[$scope.activeModuleIndex];
    //        //}
    //    }

    //    function nextModuleItem() {
    //        debugger;
    //        $scope.activeModuleIndex++;
    //        $scope.activeModule = $scope.course.modules[$scope.activeModuleIndex];
    //    }

    //    function getLastCompletedModuleIndex() {
    //        var lastCompletedIndex = -1;
    //        for (var i = 0; i < $scope.course.modules.length; i++) {
    //            if (!$scope.course.modules[i].completed)
    //                break;
                
    //            lastCompletedIndex = i;
    //        }
            
    //        return lastCompletedIndex;
    //    }


    //    function onReady() {
    //        console.log('ready - from CoursePlayerController');
    //        $scope.$apply(function () {
    //            $scope.questionsEnabled = true;
    //        });
    //        //vimeoPlayerSvc.play();
    //    }

    //    function onPlay() {
    //        console.log('play - from CoursePlayerController');
    //    }
    //    function onPause() {
    //        console.log('pause - from CoursePlayerController');
    //    }
    //    function onFinish() {
    //        console.log('finish - from CoursePlayerController');
    //        $scope.$apply(function () {
    //            $scope.questionsEnabled = true;
    //        });
    //    }

    //    //#endregion
    //}
})();
