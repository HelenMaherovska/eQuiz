﻿(function (angular) {

    angular
        .module("equizModule")
        .factory("studentDataService", studentDataService);

    studentDataService.$inject = ["$http"];

    function studentDataService($http) {

        var service = {
            getStudentInfo: getStudentInfo,
            getStudentQuizzes: getStudentQuizzes,
            getStudentComments: getStudentComments,
            saveProfileInfo: saveProfileInfo
        };

        return service;

        function getStudentInfo(studentId) {
            var promise = $http({
                url: '/Admin/Student/GetStudentInfo',
                method: "GET",
                params: {id: studentId}
            });
            return promise;
        }

        function getStudentQuizzes(studentId) {
            var promise = $http({
                url: '/Admin/Student/GetStudentQuizzes',
                method: "GET",
                params: { id: studentId }
            });
            return promise;
        }

        function getStudentComments(studentId) {
            var promise = $http({
                url: '/Admin/Student/GetStudentComments',
                method: "GET",
                params: { id: studentId }
            });
            //var promise = $http.get("/Admin/Default/GetStudentComments", studentId);
            return promise;
        }

        function saveProfileInfo(id, firstName, lastName, phone) {
            var promise = $http.post("/Admin/Student/UpdateUserInfo", { Id: id, firstName: firstName, lastName: lastName, phone: phone });
            return promise;
        }
    }

})(angular);