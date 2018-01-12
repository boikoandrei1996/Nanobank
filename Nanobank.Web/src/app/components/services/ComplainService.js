(function () {
    'use strict';

    angular
        .module('app')
        .factory('ComplainService', ComplainService);

    ComplainService.$inject = ['$http', '$httpParamSerializerJQLike', '$state'];
    function ComplainService($http, $httpParamSerializerJQLike, $state) {
        var service = {};
        var API_BASE = 'http://nanobank.azurewebsites.net'
        var specialContentType = {headers: {'Content-Type': 'application/x-www-form-urlencoded'}};


        service.GetAll = GetAll;
        service.GetById = GetById;
        service.Create = Create;
        service.Delete = Delete;

        return service;

        function GetAll() {
            return $http.get(API_BASE + '/api/complain/all').then(handleSuccess, handleError('Error getting all complains'));
        }

        function GetById(complainId) {
            return $http.get(API_BASE + '/api/complain/' + complainId).then(handleSuccess, handleError('Error getting complain by complain id'));
        }

        function Create(complain) {
            return $http.post(API_BASE + '/api/complain/add', processData(complain), specialContentType).then(handleSuccess, handleError('Error creating complain'));
        }
        function Delete(complainId) {
          // application/x-www-form-urlencoded
            return $http.delete(API_BASE + '/api/complain/' + complainId, '', specialContentType).then(handleSuccess, handleError('Error deleting complain'));
        }
        // private functions

        function processData(data){
          return $httpParamSerializerJQLike(data);
        }

        function handleSuccess(res) {
            return res.data;
        }

        function handleError(error) {
            return function () {
                return { success: false, message: error };
            };
        }
    }

})();
