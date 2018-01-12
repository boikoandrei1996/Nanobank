(function(){

  angular
    .module('app')
    .controller('ProfileController', ['UserService', 'helpService', '$timeout',
      ProfileController
    ]);

  function ProfileController(UserService, helpService, $timeout) {
    var vm = this;
    vm.one = 1;

    waitForCurrentUser();

    function waitForCurrentUser(){
      $timeout(function() {
        vm.user = UserService.currentUser;
        vm.user.phoneNumber = helpService.formatPhoneNumber(vm.user.phoneNumber)
        vm.user.ratingNegative = vm.user.ratingNegative ? vm.user.ratingNegative: 0;
        vm.user.ratingPositive = vm.user.ratingPositive ? vm.user.ratingPositive: 0;
        if (angular.isUndefined(vm.user.firstName)){
          waitForCurrentUser()
        };
      }, 500);
    }
  }

})();
