(function(){

  angular
    .module('app')
    .controller('RequestsController', ['$scope', 'DealService', '$mdDialog', 'helpService',
      RequestsController
    ]);

  function RequestsController($scope, DealService, $mdDialog, helpService) {
    var vm = this;
    vm.activated = false;
    vm.tableData = [];
    DealService.GetAll().then(function(resourse){
      vm.tableData = resourse;
      vm.activated = true;
    });
    $scope.customFilter = customFilter;
    vm.search = "";
    vm.showOpened = true;
    vm.showAssigned = true;
    vm.showUnassigned = true;
    vm.showClosed = true;

    function customFilter(deal){
      var assigned = angular.isDefined(deal.creditorUserName) && deal.creditorUserName !== null;
      var ok = vm.showOpened && !deal.isClosed || vm.showClosed && deal.isClosed;

      return vm.showUnassigned && !assigned ||
             vm.showAssigned && assigned && ok;
    }

    $scope.showAdvanced = function(ev, data) {
       $mdDialog.show({
         locals:{dealId: data},
         controller: 'DealDialogController',
         templateUrl: 'app/views/partials/dealDialog.html',
         parent: angular.element(document.body),
         controllerAs: 'vm',
         theme:"custom",
         targetEvent: ev,
         clickOutsideToClose:true,
         fullscreen: false,
       })
     };
  }

})();
