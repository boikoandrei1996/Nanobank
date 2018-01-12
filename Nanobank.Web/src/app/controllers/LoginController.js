(function () {
    'use strict';

    angular
        .module('app')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['$state', '$scope','AuthenticationService', 'FlashService', 'UserService'];
    function LoginController($state, $scope, AuthenticationService, FlashService, UserService) {
        var vm = this;
        vm.wrongData = false;
        vm.username = '';
        vm.password = '';

        vm.login = login;

        $scope.$watch('vm.username', function(newValue){
          vm.wrongData = false;
        });

        $scope.$watch('vm.password', function(newValue){
          vm.wrongData = false;
        });

        (function initController() {
            // reset login status
            AuthenticationService.ClearCredentials();
        })();

        function login() {
            vm.dataLoading = true;
            AuthenticationService.Login(vm.username, vm.password, function (response) {
                if (response.success) {
                    AuthenticationService.SetCredentials(vm.username, vm.password, response);
                    UserService.setCurrentUserAndGoToState(vm.username, 'home.dashboard')
                } else {
                    FlashService.Error(response.message);
                    vm.dataLoading = false;
                    vm.wrongData = true;
                }
            });
        };
    }

})();
