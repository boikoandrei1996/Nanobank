(function () {
    'use strict';

    angular
        .module('app')
        .controller('RegisterController', RegisterController);

    RegisterController.$inject = ['$scope','UserService', '$location', '$rootScope', 'FlashService', 'cardsService'];
    function RegisterController($scope, UserService, $location, $rootScope, FlashService, cardsService) {
        var vm = this;

        vm.user = {};
        vm.cards = cardsService.loadAll();
        console.log(vm.cards);
        vm.user.cardNumber = vm.cards[0].value;
        vm.register = register;


        function register() {
            vm.dataLoading = true;
            formatData();
            vm.user.PassportImage = vm.files.base64;
            vm.user.ImageMimeType = vm.files.filetype;
            UserService.Create(vm.user)
                .then(function (response) {
                    if (response.success) {
                        FlashService.Success('Registration successful', true);
                        $state.go('login');
                    } else {
                        FlashService.Error(response.message);
                        vm.dataLoading = false;
                    }
                });
        }

        function formatData(){
          var match = /^(\+?375|80)?(29|25|44|33)(\d{3})(\d{2})(\d{2})$/.exec(vm.user.phoneNumber);
          vm.user.phoneNumber = '375' + match.slice(2).join('');
          console.log(vm.user.phoneNumber);
        }


    var monthFormat =  buildLocaleProvider("MM/YY");
    vm.cardDateOfExpire = moment();
    vm.minDate = new Date();
    console.log(monthFormat);

      function buildLocaleProvider(formatString) {
          return {
              formatDate: function (date) {
                vm.user.cardDateOfExpire = moment(date).format(formatString);
                  if (date) return vm.user.cardDateOfExpire;
                  else return null;
              },
              parseDate: function (dateString) {
                  if (dateString) {
                      console.log('parseDate');
                      var m = moment(dateString, formatString, true);
                      console.log('parseDate', m.toDate);
                      return m.isValid() ? m.toDate() : new Date(NaN);
                  }
                  else return null;
              }
          };
      }


    vm.dateLocale =  {
                      type: 'date',
                      required: true,
                      binding: 'applicant.expectedGraduation',
                      startView: 'month',
                      label: 'Credit Card Expiry - Year/Month picker',
                      mode: 'year',
                      locale: monthFormat
                  }
    }

})();
