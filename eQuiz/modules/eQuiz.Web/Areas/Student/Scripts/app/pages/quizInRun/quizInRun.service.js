﻿/// <reference path="E:\eQuizEpam\eQuiz\modules\eQuiz.Web\Scripts/libs/angularjs/angular.js" />
(function (angular) {
    var equizModule = angular.module("equizModule");

    equizModule.factory("quizService", ["$http", function ($http) {
        var service = {
            getQuestionsById: getQuestionsByIdAjax,
            sendUserResult: sendUserResultAjax 
        };

        return service;

        function getQuestionsByIdAjax(questionId) {
            var promise = $http({
                method: "GET",
                url: "GetQuestionsByQuizId",
                params: { id: questionId }

            });

            return promise;
        };

        function sendUserResultAjax(passedQuiz) {
            var promise = $http.post("InsertQuizResult", passedQuiz);

            return promise;
        };
    }]);
})(angular);