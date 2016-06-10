$(document).ready(function(){
  
  var myModal = new Modal({
    maxWidth: 400,
    content: '<img src="/images/content/all-cvv.gif" alt="" />'
  });
  
  $('.js-popup').on('click', function(e) {
    e.preventDefault();
    myModal.open();
    if (!Modernizr.csstransforms) {
      var modal = $('.modal');
      var modalWidth = modal.width();
      console.log(modalWidth / 2);
      $(modal).css('marginLeft', -modalWidth / 2);
    }
  });
  
  //myModal.open();
  
});