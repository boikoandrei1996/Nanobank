(function(){

  angular
       .module('app')
       .controller('MainController', [
          'UserService', 'navService', '$mdSidenav', '$mdBottomSheet', '$log', '$q', '$state', '$mdToast', '$rootScope',
          MainController
       ]);

  function MainController(UserService, navService, $mdSidenav, $mdBottomSheet, $log, $q, $state, $mdToast, $rootScope) {
    var vm = this;

    vm.menuItems = [ ];
    vm.selectItem = selectItem;
    vm.toggleItemsList = toggleItemsList;
    UserService.setCurrentUser(UserService.currentUsername).then(function(){
      vm.userFullName = UserService.currentUser.firstName +" "+ UserService.currentUser.lastName;
      if (UserService.currentUser.roles.includes('Admin')){
        navService
          .loadAllItems()
          .then(function(menuItems) {
            vm.menuItems = [].concat(menuItems);
          });
      }
    });
    vm.title = $state.current.data.title;
    vm.showSimpleToast = showSimpleToast;

    navService
      .loadUserItems()
      .then(function(menuItems) {
        vm.menuItems = [].concat(menuItems);
      });

    function toggleItemsList() {
      var pending = $mdBottomSheet.hide() || $q.when(true);

      pending.then(function(){
        $mdSidenav('left').toggle();
      });
    }

    function selectItem (item) {
      $rootScope.$stateParams.deal = null;
      vm.title = item.name;
      vm.toggleItemsList();
    }

    function showSimpleToast(title) {
      $mdToast.show(
        $mdToast.simple()
          .content(title)
          .hideDelay(2000)
          .position('bottom right')
      );
    }
  }

})();
