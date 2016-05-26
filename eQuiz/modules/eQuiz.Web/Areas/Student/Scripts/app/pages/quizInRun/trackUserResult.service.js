﻿/// <reference path="E:\eQuizEpam\eQuiz\modules\eQuiz.Web\Scripts/libs/angularjs/angular.js" />
(function (angular) {
    var equizModule = angular.module("equizModule");

    equizModule.factory("trackUserResultService", function () {
        var service = {
            passedQuiz: getPassedQuiz(),
            setUserAnswers: setAnswers,
            setUserTextAnswers: setTextAnswers
        };

        return service;

        function getPassedQuiz() {
            var passedUserQuiz = {
                QuizId: null,
                StartDate: null,
                FinishDate: null,
                UserAnswers: []
            };

            return passedUserQuiz;
        };
        

        function setAnswers(index, questionId, answerId, isAutomatic, quizBlock, questionOrder) {
            if (isAutomatic) {
                var UserAnswer = {
                    QuestionId: questionId, AnswerId: answerId, AnswerText: null, AnswerTime: new Date(Date.now()), IsAutomatic: isAutomatic, QuizBlock: quizBlock, QuestionOrder: questionOrder
                };
                service.passedQuiz.UserAnswers[index] = UserAnswer;
            }
        };

        function setTextAnswers(index, questionId, isAutomatic, quizBlock, questionOrder, answerText) {
            if (!isAutomatic && answerText != null) {
                var UserAnswer = {
                    QuestionId: questionId, AnswerId: null, AnswerText: answerText, AnswerTime: new Date(Date.now()), IsAutomatic: isAutomatic, QuizBlock: quizBlock, QuestionOrder: questionOrder
                };
                service.passedQuiz.UserAnswers[index] = UserAnswer;
            }
        };
    });
})(angular);