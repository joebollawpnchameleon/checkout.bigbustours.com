$(document).ready(function(){
  $('.js-slider').slick({
    autoplay: true,
    slidesToShow: 5,
    slidesToScroll: 1,
    pauseOnHover: false,
    responsive: [
      {
        breakpoint: 500,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1
        }
      }
    ]
  });
  $('.js-slider a').on('click', function(e) {
    //e.preventDefault();
    $('.js-slider').slick('slickPause');
  });
  var vouchersContainer = $('.vouchers');
  var vouchersToggle = $('.vouchers .js-toggle');
  var vouchersInput = $('.voucher-input');
  $(vouchersToggle).on('click', function(e) {
    e.preventDefault();
    $(vouchersContainer).toggleClass('active');
    $(vouchersInput).slideToggle();
  });

  var basketContainer = $('.basket');
  var basketToggle = $('.basket-summary .js-toggle');
  $(basketToggle).on('click', function(e) {
    e.preventDefault();
    $(basketToggle).toggleClass('active');
    $(basketContainer).slideToggle();
  });
});
