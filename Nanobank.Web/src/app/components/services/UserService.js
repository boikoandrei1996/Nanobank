(function () {
    'use strict';

    angular
        .module('app')
        .factory('UserService', UserService);

    UserService.$inject = ['$http', '$httpParamSerializerJQLike', '$state'];
    function UserService($http, $httpParamSerializerJQLike, $state) {
        var service = {};
        var API_BASE = 'http://nanobank.azurewebsites.net'
        var specialContentType = {headers: {'Content-Type': 'application/x-www-form-urlencoded'}};


        service.GetAll = GetAll;
        service.GetAllUnapproved = GetAllUnapproved;
        service.GetByUsername = GetByUsername;
        service.Create = Create;
        service.Approve = Approve;
        service.Delete = Delete;
        service.Login = Login;
        service.currentUser = {};
        service.setCurrentUserAndGoToState = setCurrentUserAndGoToState;
        service.setCurrentUser = setCurrentUser;
        service.GetPassportPhoto = GetPassportPhoto;

        return service;

        function GetAll() {
            return $http.get(API_BASE + '/api/user/all').then(handleSuccess, handleError('Error getting all users'));
        }

        function GetAllUnapproved() {
            return $http.get(API_BASE + '/api/user/all/unapproved').then(handleSuccess, handleError('Error getting all users'));
        }

        function GetByUsername(username) {
            return $http.get(API_BASE + '/api/user/' + username).then(handleSuccess, handleError('Error getting user by username'));
        }

        function Create(user) {
          console.log(user);
            return $http.post(API_BASE + '/api/account/register', user).then(handleSuccess, handleError('Error creating user'));
        }

        function Approve(username) {
            // application/x-www-form-urlencoded
            return $http.put(API_BASE + '/api/account/approve/' + username, '', specialContentType).then(handleSuccess, handleError('Error updating user'));
        }

        function Delete(username) {
          // application/x-www-form-urlencoded
            return $http.delete(API_BASE + '/api/account/' + username, '', specialContentType).then(handleSuccess, handleError('Error deleting user'));
        }

        function Login(username, password){
          var loginInfo = {
            'grant_type': 'password',
            'username': username,
            'password': password
          }
          return $http.post(API_BASE + '/api/token', processData(loginInfo), specialContentType).then(handleSuccess, handleError('Error logging in user'));
        }

        function GetPassportPhoto(username){
          return $http.get(API_BASE + '/api/user/' + username + '/passport/photo').then(handleSuccess, handleError('Error getting passport photo'));
        }

        function setCurrentUserAndGoToState(username, toState){
          setCurrentUser(username).then(
            $state.go(toState)
          )
        }

        function setCurrentUser(username){
          return GetByUsername(username).then(function (response){
            service.currentUser = response;
          });
        }
        // private functions

        function processData(data){
          return $httpParamSerializerJQLike(data);
        }

        function handleSuccess(res) {
            console.log(res.data);
            return res.data;
        }

        function handleError(error) {
            return function () {
                return { success: false, message: error };
            };
        }
    }

})();
