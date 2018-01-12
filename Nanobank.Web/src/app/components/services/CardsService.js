(function(){
  'use strict';

  angular.module('app')
    .service('cardsService', [
      cardsService
    ]);


  function cardsService(){
    var allCards = ['0001 0001 0001 0001',
      '0002 0002 0002 0002',
      '0003 0003 0003 0003',
      '0004 0004 0004 0004',
      '0005 0005 0005 0005',
      '0006 0006 0006 0006',
      '0007 0007 0007 0007',
      '0008 0008 0008 0008',
      '0009 0009 0009 0009',
      '0010 0010 0010 0010'];

    return {
      loadAll : function() {
        return allCards.map(function(card) {
          return {
            value: card.replace(/ /g,''),
            display: card
          };
        });
      }
    };
  }

})();
