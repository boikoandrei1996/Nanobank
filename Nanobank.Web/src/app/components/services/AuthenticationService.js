(function () {
    'use strict';

    angular
        .module('app')
        .factory('AuthenticationService', AuthenticationService);

    AuthenticationService.$inject = ['$http', '$cookies', '$rootScope', '$timeout', 'UserService'];
    function AuthenticationService($http, $cookies, $rootScope, $timeout, UserService) {
        var service = {};

        service.Login = Login;
        service.SetCredentials = SetCredentials;
        service.ClearCredentials = ClearCredentials;

        return service;

        function Login(username, password, callback) {
            return UserService.Login(username, password)
               .then(function (response) {
                 if (angular.isUndefined(response.success)){
                   console.log('AuthenticationService Login success', response);
                   response.success = true;
                 }else {
                   console.log('AuthenticationService Login error', response);
                   response.success = false;
                 }
                 callback(response);
               });
        }

        function SetCredentials(username, password, response) {
            var authdata = response.access_token;
            console.log();

            $rootScope.globals = {
                currentUser: {
                    username: username,
                    authdata: authdata,
                }
            };

            // set default auth header for http requests
            $http.defaults.headers.common['Authorization'] = response.token_type+ " " + authdata;

            // store user details in globals cookie that keeps user logged in for 1 week (or until they logout)
            var cookieExp = new Date();
            cookieExp.setDate(cookieExp.getDate() + response.expires_in);
            $cookies.putObject('globals', $rootScope.globals, { expires: cookieExp });
        }

        function ClearCredentials() {
            $rootScope.globals = {};
            $cookies.remove('globals');
            $http.defaults.headers.common.Authorization = 'Basic';
            UserService.currentUsername = '';
            UserService.currentUser = {};
        }
    }

})();
