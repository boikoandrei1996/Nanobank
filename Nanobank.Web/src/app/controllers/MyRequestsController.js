(function(){

  angular
    .module('app')
    .controller('MyRequestsController', ['$scope', '$state','DealService', '$mdDialog', 'helpService', 'UserService',
      MyRequestsController
    ]);

  function MyRequestsController($scope, $state,DealService, $mdDialog, helpService, UserService) {
    var vm = this;
    vm.activated = false;
    vm.tableData = [];

    DealService.GetAllUserDeals(UserService.currentUsername).then(function(resourse){
      vm.ownerTableData = resourse.asOwner;
      vm.creditorTableData = resourse.asCreditor;
      vm.activated = true;
    });
    $scope.customFilter = customFilter;
    vm.search = "";
    vm.showOpened = true;
    vm.showUnassigned = true;
    vm.showClosed = true;
    vm.showAssigned = true;
    vm.goWithData = goWithData;
    vm.formatDataPeriod = helpService.formatDatePeriod;

    function customFilter(deal){
      var assigned = angular.isDefined(deal.creditorUserName) && deal.creditorUserName !== null;
      var ok = vm.showOpened && !deal.isClosed || vm.showClosed && deal.isClosed;

      return vm.showUnassigned && !assigned ||
             vm.showAssigned && assigned && ok;
    }

    function goWithData(data){
      $state.go('home.deal', {deal: data});
      console.log(data);
    }
  }

})();
