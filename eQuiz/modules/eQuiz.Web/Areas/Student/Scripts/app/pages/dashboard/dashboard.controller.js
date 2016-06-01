﻿
(function (angular) {
    var equizModule = angular.module("equizModule");

    equizModule.controller("dashboardCtrl", ["$scope", "dashboardService", function ($scope, dashboardService) {
        $scope.allQuizes = null;

        $scope.isLoading = true;

        $scope.setToLocalStorage = function (durationValue) {
            localStorage.setItem('duration', durationValue)
        }

        activate();
        function activate() {
            var quizPromise = dashboardService.getQuizzes();
            quizPromise.success(function (data) {
                $scope.allQuizes = data;
                $scope.isLoading = false;
            });
            
           
        };
    }]);
})(angular);