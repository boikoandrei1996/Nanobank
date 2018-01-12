(function(){

  angular
    .module('app')
    .controller('DealDialogController', ['dealId', 'DealService', '$scope', '$state','$mdDialog', 'helpService',
      DealDialogController
    ]);

  function DealDialogController(dealId, DealService, $scope, $state, $mdDialog, helpService) {
    var vm = this;
    console.log('deal', dealId);
    DealService.GetById(dealId).then(function (response){
      vm.deal = response;
      vm.deal.dateInWords = helpService.formatDatePeriod(vm.deal.dealDurationInMonth);
    });
    
    $scope.hide = function() {
      $mdDialog.hide();
    };

    $scope.cancel = function() {
      $mdDialog.cancel();
    };

    $scope.answer = function(answer) {
      $mdDialog.hide(answer);
    };

  }

})();
