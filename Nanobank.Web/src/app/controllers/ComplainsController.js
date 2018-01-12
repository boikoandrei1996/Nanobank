(function(){

  angular
    .module('app')
    .controller('ComplainsController', ['$scope', 'ComplainService', '$mdDialog', 'helpService',
      ComplainsController
    ]);

  function ComplainsController($scope, ComplainService, $mdDialog, helpService) {
    var vm = this;
    vm.activated = false;
    vm.tableData = [];
    ComplainService.GetAll().then(function(resourse){
      vm.tableData = resourse;
      console.log(resourse);
      vm.activated = true;
    });
    $scope.customFilter = customFilter;
    vm.search = "";
    vm.showOpened = true;
    vm.showAssigned = true;
    vm.showUnassigned = true;
    vm.showClosed = true;
    vm.formatDate = formatDate;

    function customFilter(deal){
      var assigned = angular.isDefined(deal.creditorUserName) && deal.creditorUserName !== null;
      var ok = vm.showOpened && !deal.isClosed || vm.showClosed && deal.isClosed;

      return vm.showUnassigned && !assigned ||
             vm.showAssigned && assigned && ok;
    }

    function formatDate(date){
      console.log(date);
      return moment(date).format("DD.MM.YYYY")
    }

    function deleteDeal(dealId){
      console.log(dealId);
    }

    $scope.showAdvanced = function(ev, data) {
       $mdDialog.show({
         locals:{dealId: data.dealId},
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
