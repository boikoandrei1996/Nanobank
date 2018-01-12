(function(){

  angular
    .module('app')
    .controller('UsersController', ['$scope', 'UserService', '$mdDialog', 'helpService',
      UsersController
    ]);

  function UsersController($scope, UserService, $mdDialog, helpService) {
    var vm = this;
    vm.activated = false;
    vm.tableData = [];
    UserService.GetAll().then(function(resourse){
      resourse.forEach(function(row){
        row.phoneNumber = helpService.formatPhoneNumber(row.phoneNumber);
        vm.tableData.push(row);
      });
      vm.activated = true;
    });
    $scope.customFilter = customFilter;
    vm.search = "";
    vm.showApproved = true;
    vm.showUnapproved = true;

    $scope.showAdvanced = function(ev, data) {
       $mdDialog.show({
         locals:{user: data},
         controller: 'UserDialogController',
         templateUrl: 'app/views/partials/userDialog.html',
         parent: angular.element(document.body),
         controllerAs: 'vm',
         theme:"custom",
         targetEvent: ev,
         clickOutsideToClose:true,
         fullscreen: false
       })
       .then(function(answer) {
         $scope.status = 'You said the information was "' + answer + '".';
         console.log($scope.status);
       }, function() {
         $scope.status = 'You cancelled the dialog.';
         console.log($scope.status);
       });
     };

    function customFilter(user){
      return vm.showApproved && user.isApproved || vm.showUnapproved && !user.isApproved;
    }
  }

})();
