(function(){
  'use strict';

  angular.module('app')
          .service('helpService', ['$mdDialog', '$mdToast',
          helpService
  ]);

  function helpService($mdDialog, $mdToast){
    var service = {};

    service.formatPhoneNumber = formatPhoneNumber;
    service.formatRating = formatRating;
    service.formatDatePeriod = formatDatePeriod;
    service.getConfirmDialog = getConfirmDialog;
    service.getDialogWithInput = getDialogWithInput;
    service.formatImageType = formatImageType;
    service.showSimpleToast = showSimpleToast;

    function formatPhoneNumber(phoneNumber){
      var match = /^(\+?375|80)?(29|25|44|33)(\d{3})(\d{2})(\d{2})$/.exec(phoneNumber);
      if (angular.isDefined(match) && match != null){
        return '+375' + match.slice(2).join('') ;
      }else {
          return phoneNumber;
      }
    };

    function formatRating(rating, isPositive){
      var match = /^[\-\+]?(\d+)$/.exec(rating);
      var sign = isPositive ? "+":"-";
      if (angular.isUndefined(match) || match == null){
        match = [0,0];
      }
      return sign + match[1];
    }

    function formatDatePeriod(monthesCount){
      var answer = '';
      var monthes = [1, 6, 12, 18];
      var inWords = ['месяц','полгода', 'год', 'полтора года']

      if (monthes.indexOf(monthesCount) != -1){
        answer = inWords[monthes.indexOf(monthesCount)];
      }else if (monthesCount % 10 == 1) {
        answer = monthesCount + ' месяц';
      }else if ([2,3,4].indexOf(monthesCount % 10)!=-1 && (monthesCount<10 || monthesCount>20)) {
        answer = monthesCount + ' месяца';
      }else {
        answer = monthesCount + ' месяцев';
      }

      return answer;
    }

    function getConfirmDialog(title, textContent, ok){
      var confirm = $mdDialog.confirm({
                    onComplete: function afterShowAnimation() {
                        var $dialog = angular.element(document.querySelector('md-dialog'));
                        var $actionsSection = $dialog.find('md-dialog-actions');
                        var $cancelButton = $actionsSection.children()[0];
                        var $confirmButton = $actionsSection.children()[1];
                        angular.element($confirmButton).addClass('md-raised md-accent');
                        angular.element($cancelButton).addClass('md-accent');
                    }
      })
            .title(title)
            .textContent(textContent)
            .ariaLabel('Lucky day')
            .targetEvent(null)
            .ok('Да, ' + ok)
            .cancel('Отмена');

      return confirm;
    }

    function getDialogWithInput(title, textContent, placeholder, initialValue, ok){
      var prompt = $mdDialog.prompt({
                    onComplete: function afterShowAnimation() {
                        var $dialog = angular.element(document.querySelector('md-dialog'));
                        var $actionsSection = $dialog.find('md-dialog-actions');
                        var $cancelButton = $actionsSection.children()[0];
                        var $confirmButton = $actionsSection.children()[1];
                        angular.element($confirmButton).addClass('md-raised md-accent');
                        angular.element($cancelButton).addClass('md-accent');
                    }
      })
      .title(title)
      .textContent(textContent)
      .placeholder(placeholder)
      .ariaLabel('Dog name')
      .initialValue(initialValue)
      .targetEvent(null)
      .required(true)
      .ok(ok)
      .cancel('отмена');

      return prompt;
    }

    function formatImageType(type){
      var match = /^(image\/)?(\w+)$/.exec(type);
      return "image/"+match[2];
    }

    function showSimpleToast(title) {
      $mdToast.show(
        $mdToast.simple()
          .content(title)
          .hideDelay(2000)
          .position('bottom right')
      );
    }

    return service;
  }

})();
