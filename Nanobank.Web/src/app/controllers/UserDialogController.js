(function(){

  angular
    .module('app')
    .controller('UserDialogController', ['user', 'UserService', '$scope', '$state','$mdDialog', 'cardsService', 'helpService',
      UserDialogController
    ]);

  function UserDialogController(user, UserService, $scope, $state, $mdDialog, cardsService, helpService) {
    var vm = this;
    vm.user = user;
    vm.cards = cardsService.loadAll();
    vm.cards.forEach(function(card){
      if (card.value == vm.user.cardNumber){
        vm.user.cardNumber = card.display;
      };
    });

    UserService.GetPassportPhoto(vm.user.userName).then(function(image){
      var imageMimeType = helpService.formatImageType(image.imageMimeType);
      var passportPhoto = image.passportImage;
      $scope.imagePath = "data:"+ imageMimeType + ";base64,"+ passportPhoto;
    });


    vm.user.ratingPositive = helpService.formatRating(vm.user.ratingPositive, true);
    vm.user.ratingNegative = helpService.formatRating(vm.user.ratingNegative, false);

    $scope.hide = function() {
      $mdDialog.hide();
    };

    $scope.cancel = function() {
      $mdDialog.cancel();
    };

    $scope.answer = function(answer) {
      $mdDialog.hide(answer);
    };

    $scope.showConfirm = function(ev, forDeletion) {
      var title = forDeletion ? 'Вы действительно хотите удалить аккаунт данного пользователя?':'Вы действительно хотите утвердить аккаунт данного пользователя?'
      var textContent = forDeletion ? 'После подтверждения удаления учётная запись пользователя сотрётся из базы данных.' : 'После подтверждения действия пользователю предоставится доступ к ресурсам приложения.'
      var ok = forDeletion ? 'удалить.' : 'подтвердить.'

      $mdDialog.show(helpService.getConfirmDialog(title, textContent, ok)).then(function() {
        makeUserAction(forDeletion).then(function(response){
          $state.reload();
        });
      }, function() {
        // console.log('You decided to keep your debt.');
        // console.log(vm.user);
      });


      function makeUserAction(deletion){
        if (deletion){
          return UserService.Delete(vm.user.userName);
        }else {
          return UserService.Approve(vm.user.userName);
        }
      }
    };
  }

})();
