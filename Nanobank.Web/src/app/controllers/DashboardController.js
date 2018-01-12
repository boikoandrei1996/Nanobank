(function(){

  angular
    .module('app')
    .controller('DashboardController', ['DealService', 'helpService',
      DashboardController
    ]);

  function DashboardController(DealService, helpService) {
    var vm = this;

    DealService.GetAllOpen().then(function(resourse){
      vm.deals = resourse;
      vm.deals.forEach(function(deal, i){
        vm.deals[i].dealDurationInWords = helpService.formatDatePeriod(deal.dealDurationInMonth)
      });
      vm.separatedIntoThreeData = breakDataIntoThree();
      vm.activated = true;

    });

    function breakDataIntoThree(){
      var count = 3;
      var i = 0;
      var separatedValues = [];
      while (i < vm.deals.length){
          separatedValues.push(vm.deals.slice(i, i+3));
          i += 3;
      }
      return separatedValues;
    }
  }

})();
