(function ($) {
    "use strict";

    var passiveSupported = false;

    try {
        var options = Object.defineProperty({}, 'passive', {
            get: function() {
                passiveSupported = true;
            }
        });

        window.addEventListener('test', null, options);
    } catch(err) {}

    var DIRECTION = null;

    function direction() {
        if (DIRECTION === null) {
            DIRECTION = getComputedStyle(document.body).direction;
        }

        return DIRECTION;
    }

    function isRTL() {
        return direction() === 'rtl';
    }

    /*
    // initialize custom numbers
    */
    $(function () {
        $('.input-number').customNumber();
    });


    /*
    // topbar dropdown
    */
    $(function() {
        $('.topbar-dropdown__btn').on('click', function() {
            $(this).closest('.topbar-dropdown').toggleClass('topbar-dropdown--opened');
        });

        $(document).on('click', function (event) {
            $('.topbar-dropdown')
                .not($(event.target).closest('.topbar-dropdown'))
                .removeClass('topbar-dropdown--opened');
        });
    });


    /*
    // dropcart, drop search
    */
    $(function() {
        $('.indicator--trigger--click .indicator__button').on('click', function(event) {
            event.preventDefault();

            var dropdown = $(this).closest('.indicator');

            if (dropdown.is('.indicator--opened')) {
                dropdown.removeClass('indicator--opened');
            } else {
                dropdown.addClass('indicator--opened');
                dropdown.find('.drop-search__input').focus();
            }
        });

        $('.indicator--trigger--click .drop-search__input').on('keydown', function(event) {
            if (event.which === 27) {
                $(this).closest('.indicator').removeClass('indicator--opened');
            }
        });

        $(document).on('click', function (event) {
            $('.indicator')
                .not($(event.target).closest('.indicator'))
                .removeClass('indicator--opened');
        });
    });


    /*
    // product tabs
    */
    $(function () {
        $('.product-tabs').each(function (i, element) {
            $('.product-tabs__list', element).on('click', '.product-tabs__item', function (event) {
                event.preventDefault();

                var tab = $(this);
                var content = $('.product-tabs__pane' + $(this).attr('href'), element);

                if (content.length) {
                    $('.product-tabs__item').removeClass('product-tabs__item--active');
                    tab.addClass('product-tabs__item--active');

                    $('.product-tabs__pane').removeClass('product-tabs__pane--active');
                    content.addClass('product-tabs__pane--active');
                }
            });

            var currentTab = $('.product-tabs__item--active', element);
            var firstTab = $('.product-tabs__item:first', element);

            if (currentTab.length) {
                currentTab.trigger('click');
            } else {
                firstTab.trigger('click');
            }
        });
    });


    /*
    // megamenu position
    */
    $(function() {
        $('.nav-panel__nav-links .nav-links__item').on('mouseenter', function() {
            var megamenu = $(this).find('.nav-links__megamenu');

            if (megamenu.length) {
                var container = megamenu.offsetParent();
                var containerWidth = container.width();
                var megamenuWidth = megamenu.width();

                if (isRTL()) {
                    var itemPosition = containerWidth - ($(this).position().left + $(this).width());
                    var megamenuPosition = Math.round(Math.min(itemPosition, containerWidth - megamenuWidth));

                    megamenu.css('right', megamenuPosition + 'px');
                } else {
                    var itemPosition = $(this).position().left;
                    var megamenuPosition = Math.round(Math.min(itemPosition, containerWidth - megamenuWidth));

                    megamenu.css('left', megamenuPosition + 'px');
                }
            }
        });
    });


    /*
    // mobile search
    */
    $(function() {
        var mobileSearch = $('.mobile-header__search');

        if (mobileSearch.length) {
            $('.indicator--mobile-search .indicator__button').on('click', function() {
                if (mobileSearch.is('.mobile-header__search--opened')) {
                    mobileSearch.removeClass('mobile-header__search--opened');
                } else {
                    mobileSearch.addClass('mobile-header__search--opened');
                    mobileSearch.find('input')[0].focus();
                }
            });

            mobileSearch.find('.mobile-header__search-button--close').on('click', function() {
                mobileSearch.removeClass('mobile-header__search--opened');
            });

            document.addEventListener('click', function(event) {
                if (!$(event.target).closest('.indicator--mobile-search, .mobile-header__search').length) {
                    mobileSearch.removeClass('mobile-header__search--opened');
                }
            }, true);
        }
    });


    /*
    // departments, sticky header
    */
    $(function() {
        /*
        // departments
        */
        var CDepartments = function(element) {
            var self = this;

            element.data('departmentsInstance', self);

            this.element = element;
            this.body = this.element.find('.departments__body');
            this.button = this.element.find('.departments__button');
            this.mode = this.element.is('.departments--fixed') ? 'fixed' : 'normal';
            this.fixedBy = $(this.element.data('departments-fixed-by'));
            this.fixedHeight = 0;

            if (this.mode === 'fixed' && this.fixedBy.length) {
                this.fixedHeight = this.fixedBy.offset().top - this.body.offset().top + this.fixedBy.outerHeight();
                this.body.css('height', this.fixedHeight + 'px');
            }

            this.button.on('click', function(event) {
                self.clickOnButton(event);
            });

            $('.departments__links-wrapper', this.element).on('transitionend', function (event) {
                if (event.originalEvent.propertyName === 'height') {
                    $(this).css('height', '');
                    $(this).closest('.departments').removeClass('departments--transition');
                }
            });

            document.addEventListener('click', function(event) {
                self.element.not($(event.target).closest('.departments')).each(function() {
                    if (self.element.is('.departments--opened')) {
                        self.element.data('departmentsInstance').close();
                    }
                });
            }, true);
        };
        CDepartments.prototype.clickOnButton = function(event) {
            event.preventDefault();

            if (this.element.is('.departments--opened')) {
                this.close();
            } else {
                this.open();
            }
        };
        CDepartments.prototype.setMode = function(mode) {
            this.mode = mode;

            if (this.mode === 'normal') {
                this.element.removeClass('departments--fixed');
                this.element.removeClass('departments--opened');
                this.body.css('height', 'auto');
            }
            if (this.mode === 'fixed') {
                this.element.addClass('departments--fixed');
                this.element.addClass('departments--opened');
                this.body.css('height', this.fixedHeight + 'px');
            }
        };
        CDepartments.prototype.close = function() {
            if (this.element.is('.departments--fixed')) {
                return;
            }

            var content = this.element.find('.departments__links-wrapper');
            var startHeight = content.height();

            content.css('height', startHeight + 'px');
            this.element
                .addClass('departments--transition')
                .removeClass('departments--opened');

            content.css('height', '');
        };
        CDepartments.prototype.open = function() {
            var content = this.element.find('.departments__links-wrapper');
            var startHeight = content.height();

            this.element
                .addClass('departments--transition')
                .addClass('departments--opened');

            var endHeight = content.height();

            content.css('height', startHeight + 'px');
            content.css('height', endHeight + 'px');
        };

        var departments = new CDepartments($('.departments'));


        /*
        // sticky header
        */
        var nav = $('.nav-panel--sticky');

        if (nav.length) {
            var departmentsMode = departments.mode;
            var defaultPosition = nav.offset().top;
            var stuck = false;

            window.addEventListener('scroll', function() {
                if (window.pageYOffset > defaultPosition) {
                    if (!stuck) {
                        nav.addClass('nav-panel--stuck');
                        stuck = true;

                        if (departmentsMode === 'fixed') {
                            departments.setMode('normal');
                        }
                    }
                } else {
                    if (stuck) {
                        nav.removeClass('nav-panel--stuck');
                        stuck = false;

                        if (departmentsMode === 'fixed') {
                            departments.setMode('fixed');
                        }
                    }
                }
            }, passiveSupported ? {passive: true} : false);
        }
    });


    /*
    // block slideshow
    */
    $(function() {
        $('.block-slideshow .owl-carousel').owlCarousel({
            items: 1,
            nav: false,
            dots: true,
            loop: true,
            rtl: isRTL()
        });
    });


    /*
    // block brands carousel
    */
    $(function() {
        $('.block-brands__slider .owl-carousel').owlCarousel({
            nav: false,
            dots: false,
            loop: true,
            rtl: isRTL(),
            responsive: {
                1200: {items: 6},
                992: {items: 5},
                768: {items: 4},
                576: {items: 3},
                0: {items: 2}
            }
        });
    });


    /*
    // block posts carousel
    */
    $(function() {
        $('.block-posts').each(function() {
            var layout = $(this).data('layout');
            var options = {
                margin: 30,
                nav: false,
                dots: false,
                loop: true,
                rtl: isRTL()
            };
            var layoutOptions = {
                'grid-nl': {

                    responsive: {
                        992: {items: 3},
                        768: {items: 2},
                        0: {items: 1}
                    }
                },
                'list-sm': {
                    responsive: {
                        992: {items: 2},
                        0: {items: 1}
                    }
                }
            };
            var owl = $('.block-posts__slider .owl-carousel');

            owl.owlCarousel($.extend({}, options, layoutOptions[layout]));

            $(this).find('.block-header__arrow--left').on('click', function() {
                owl.trigger('prev.owl.carousel', [500]);
            });
            $(this).find('.block-header__arrow--right').on('click', function() {
                owl.trigger('next.owl.carousel', [500]);
            });
        });
    });


    /*
    // teammates
    */
    $(function() {
        $('.teammates .owl-carousel').owlCarousel({
            nav: false,
            dots: true,
            rtl: isRTL(),
            responsive: {
                768: {items: 3, margin: 32},
                380: {items: 2, margin: 24},
                0: {items: 1}
            }
        });
    });

    /*
    // quickview
    */
    var quickview = {
        cancelPreviousModal: function() {},
        clickHandler: function() {
            var modal = $('#quickview-modal');
            var button = $(this);
            var doubleClick = button.is('.product-card__quickview--preload');

            quickview.cancelPreviousModal();

            if (doubleClick) {
                return;
            }

            button.addClass('product-card__quickview--preload');

            var xhr = null;
            // timeout ONLY_FOR_DEMO!
            var timeout = setTimeout(function() {
                xhr = $.ajax({
                    url: 'quickview.html',
                    success: function(data) {
                        quickview.cancelPreviousModal = function() {};
                        button.removeClass('product-card__quickview--preload');

                        modal.find('.modal-content').html(data);
                        modal.find('.quickview__close').on('click', function() {
                            modal.modal('hide');
                        });
                        modal.modal('show');
						
						modal.find('.product-gallery__featured').lightGallery({
							selector: 'a',
							loop: false,
							mousewheel: false,
							download: false,
							actualSize: false
						});
                    }
                });
            }, 1000);

            quickview.cancelPreviousModal = function() {
                button.removeClass('product-card__quickview--preload');

                if (xhr) {
                    xhr.abort();
                }

                // timeout ONLY_FOR_DEMO!
                clearTimeout(timeout);
            };
        }
    };

    $(function () {
        var modal = $('#quickview-modal');

        modal.on('shown.bs.modal', function() {
            modal.find('.product').each(function () {
                var gallery = $(this).find('.product-gallery');

                if (gallery.length > 0) {
                    initProductGallery(gallery[0], $(this).data('layout'));
                }
            });

            $('.input-number', modal).customNumber();
        });

        $('.product-card__quickview').on('click', function() {
            quickview.clickHandler.apply(this, arguments);
        });
    });


    /*
    // products carousel
    */
    $(function() {
        $('.block-products-carousel').each(function() {
            var layout = $(this).data('layout');
            var options = {
                items: 4,
                margin: 14,
                nav: false,
                dots: false,
                loop: true,
                stagePadding: 1,
                rtl: isRTL()
            };
            var layoutOptions = {
                'grid-3': {
                    responsive: {
                        1200: { items: 3, margin: 14 },
                        992: { items: 3, margin: 10 },
                        768: { items: 3, margin: 10 },
                        576: { items: 3, margin: 10 },
                        475: { items: 3, margin: 10 },
                        0: { items:3 }
                    }
                },
                'grid-4': {
                    responsive: {
                        1200: {items: 4, margin: 14},
                        992:  {items: 4, margin: 10},
                        768:  {items: 3, margin: 10},
                        576:  {items: 2, margin: 10},
                        475:  {items: 2, margin: 10},
                        0:    {items: 2}
                    }
                },
                'grid-4-sm': {
                    responsive: {
                        1200: {items: 4, margin: 14},
                        992:  {items: 3, margin: 10},
                        768:  {items: 3, margin: 10},
                        576:  {items: 2, margin: 10},
                        475:  {items: 2, margin: 10},
                        0:    {items: 1}
                    }
                },
                'grid-5': {
                    items: 4,
                    responsive: {
                        1200: {items: 4, margin: 12},
                        992:  {items: 4, margin: 10},
                        768:  {items: 2, margin: 10},
                        576:  {items: 2, margin: 10},
                        475:  {items: 2, margin: 10},
                        0:    {items: 1}
                    }
                },
                'grid-6': {
                    items: 4,
                    responsive: {
                        1200: {items: 4, margin: 12},
                        992:  {items: 4, margin: 10},
                        768: { items: 4, margin: 10},
                        576: { items: 2, margin: 10},
                        475: { items: 2, margin: 10},
                        0: { items: 2}
                    }
                },
                'horizontal': {
                    items: 3,
                    responsive: {
                        1200: {items: 3, margin: 14},
                        992:  {items: 3, margin: 10},
                        768:  {items: 2, margin: 10},
                        576:  {items: 1},
                        475:  {items: 1},
                        0:    {items: 1}
                    }
                },
            };
            var owl = $('.owl-carousel', this);
            var cancelPreviousTabChange = function() {};

            owl.owlCarousel($.extend({}, options, layoutOptions[layout]));

            $(this).find('.block-header__group').on('click', function(event) {
                var block = $(this).closest('.block-products-carousel');

                event.preventDefault();

                if ($(this).is('.block-header__group--active')) {
                    return;
                }

                cancelPreviousTabChange();

                block.addClass('block-products-carousel--loading');
                $(this).closest('.block-header__groups-list').find('.block-header__group--active').removeClass('block-header__group--active');
                $(this).addClass('block-header__group--active');

                // timeout ONLY_FOR_DEMO! you can replace it with an ajax request
                var timer;
                timer = setTimeout(function() {
                    var items = block.find('.owl-carousel .owl-item:not(".cloned") .block-products-carousel__column');

                    /*** this is ONLY_FOR_DEMO! / start */
                    /**/ var itemsArray = items.get();
                    /**/ var newItemsArray = [];
                    /**/
                    /**/ while (itemsArray.length > 0) {
                    /**/     var randomIndex = Math.floor(Math.random() * itemsArray.length);
                    /**/     var randomItem = itemsArray.splice(randomIndex, 1)[0];
                    /**/
                    /**/     newItemsArray.push(randomItem);
                    /**/ }
                    /**/ items = $(newItemsArray);
                    /*** this is ONLY_FOR_DEMO! / end */

                    block.find('.owl-carousel')
                        .trigger('replace.owl.carousel', [items])
                        .trigger('refresh.owl.carousel')
                        .trigger('to.owl.carousel', [0, 0]);

                    $('.product-card__quickview', block).on('click', function() {
                        quickview.clickHandler.apply(this, arguments);
                    });

                    block.removeClass('block-products-carousel--loading');
                }, 1000);
                cancelPreviousTabChange = function() {
                    // timeout ONLY_FOR_DEMO!
                    clearTimeout(timer);
                    cancelPreviousTabChange = function() {};
                };
            });

            $(this).find('.block-header__arrow--left').on('click', function() {
                owl.trigger('prev.owl.carousel', [500]);
            });
            $(this).find('.block-header__arrow--right').on('click', function() {
                owl.trigger('next.owl.carousel', [500]);
            });
        });
    });


    $(function () {
        $('.block-products-carousel_1').each(function () {
            var layout = $(this).data('layout');
            var options = {
                items: 4,
                margin: 14,
                nav: false,
                dots: false,
                loop: true,
                stagePadding: 1,
                rtl: isRTL()
            };
            var layoutOptions = {
                'grid-3': {
                    responsive: {
                        1200: { items: 3, margin: 14 },
                        992: { items: 3, margin: 10 },
                        768: { items: 3, margin: 10 },
                        576: { items: 3, margin: 10 },
                        475: { items: 3, margin: 10 },
                        0: { items: 3 }
                    }
                },
                'grid-4': {
                    responsive: {
                        1200: { items: 4, margin: 14 },
                        992: { items: 4, margin: 10 },
                        768: { items: 3, margin: 10 },
                        576: { items: 2, margin: 10 },
                        475: { items: 2, margin: 10 },
                        0: { items: 2 }
                    }
                },
                'grid-4-sm': {
                    responsive: {
                        1200: { items: 4, margin: 14 },
                        992: { items: 3, margin: 10 },
                        768: { items: 3, margin: 10 },
                        576: { items: 2, margin: 10 },
                        475: { items: 2, margin: 10 },
                        0: { items: 1 }
                    }
                },
                'grid-5': {
                    items: 5,
                    responsive: {
                        1200: { items: 5, margin: 12 },
                        992: { items: 5, margin: 10 },
                        768: { items: 2, margin: 10 },
                        576: { items: 2, margin: 10 },
                        475: { items: 2, margin: 10 },
                        0: { items: 1 }
                    }
                },
                'grid-6': {
                    items: 4,
                    responsive: {
                        1200: { items: 4, margin: 12 },
                        992: { items: 4, margin: 10 },
                        768: { items: 4, margin: 10 },
                        576: { items: 2, margin: 10 },
                        475: { items: 2, margin: 10 },
                        0: { items: 2 }
                    }
                },
                'horizontal': {
                    items: 3,
                    responsive: {
                        1200: { items: 3, margin: 14 },
                        992: { items: 3, margin: 10 },
                        768: { items: 2, margin: 10 },
                        576: { items: 1 },
                        475: { items: 1 },
                        0: { items: 1 }
                    }
                },
            };
            var owl = $('.owl-carousel', this);
            var cancelPreviousTabChange = function () { };

            owl.owlCarousel($.extend({}, options, layoutOptions[layout]));

            $(this).find('.block-header__group').on('click', function (event) {
                var block = $(this).closest('.block-products-carousel_1');

                event.preventDefault();

                if ($(this).is('.block-header__group--active')) {
                    return;
                }

                cancelPreviousTabChange();

                block.addClass('block-products-carousel_1--loading');
                $(this).closest('.block-header__groups-list').find('.block-header__group--active').removeClass('block-header__group--active');
                $(this).addClass('block-header__group--active');

                // timeout ONLY_FOR_DEMO! you can replace it with an ajax request
                var timer;
                timer = setTimeout(function () {
                    var items = block.find('.owl-carousel .owl-item:not(".cloned") .block-products-carousel_1__column');

                    /*** this is ONLY_FOR_DEMO! / start */
                    /**/ var itemsArray = items.get();
                    /**/ var newItemsArray = [];
                    /**/
                    /**/ while (itemsArray.length > 0) {
                    /**/     var randomIndex = Math.floor(Math.random() * itemsArray.length);
                    /**/     var randomItem = itemsArray.splice(randomIndex, 1)[0];
                    /**/
                    /**/     newItemsArray.push(randomItem);
                        /**/
}
                    /**/ items = $(newItemsArray);
                    /*** this is ONLY_FOR_DEMO! / end */

                    block.find('.owl-carousel')
                        .trigger('replace.owl.carousel', [items])
                        .trigger('refresh.owl.carousel')
                        .trigger('to.owl.carousel', [0, 0]);

                    $('.product-card__quickview', block).on('click', function () {
                        quickview.clickHandler.apply(this, arguments);
                    });

                    block.removeClass('block-products-carousel_1--loading');
                }, 1000);
                cancelPreviousTabChange = function () {
                    // timeout ONLY_FOR_DEMO!
                    clearTimeout(timer);
                    cancelPreviousTabChange = function () { };
                };
            });

            $(this).find('.block-header__arrow--left').on('click', function () {
                owl.trigger('prev.owl.carousel', [500]);
            });
            $(this).find('.block-header__arrow--right').on('click', function () {
                owl.trigger('next.owl.carousel', [500]);
            });
        });
    });


    /*



    /*
    // product gallery
    */
    var initProductGallery = function(element, layout) {
        layout = layout !== undefined ? layout : 'standard';

        var options = {
            dots: false,
            margin: 10,
            rtl: isRTL()
        };
        var layoutOptions = {
            standard: {
                responsive: {
                    1200: {items: 5},
                    992: {items: 4},
                    768: {items: 3},
                    480: {items: 5},
                    380: {items: 4},
                    0: {items: 3}
                }
            },
            sidebar: {
                responsive: {
                    768: {items: 4},
                    480: {items: 5},
                    380: {items: 4},
                    0: {items: 3}
                }
            },
            columnar: {
                responsive: {
                    768: {items: 4},
                    480: {items: 5},
                    380: {items: 4},
                    0: {items: 3}
                }
            },
            quickview: {
                responsive: {
                    1200: {items: 5},
                    768: {items: 4},
                    480: {items: 5},
                    380: {items: 4},
                    0: {items: 3}
                }
            }
        };

        var gallery = $(element);

        var image = gallery.find('.product-gallery__featured .owl-carousel');
        var carousel = gallery.find('.product-gallery__carousel .owl-carousel');

        image
            .owlCarousel({items: 1, dots: false, rtl: isRTL()})
            .on('changed.owl.carousel', syncPosition);

        carousel
            .on('initialized.owl.carousel', function () {
                carousel.find('.product-gallery__carousel-item').eq(0).addClass('product-gallery__carousel-item--active');
            })
            .owlCarousel($.extend({}, options, layoutOptions[layout]));

        carousel.on('click', '.owl-item', function(e){
            e.preventDefault();

            image.data('owl.carousel').to($(this).index(), 300, true);
        });

        function syncPosition (el) {
            var current = el.item.index;

            carousel
                .find('.product-gallery__carousel-item')
                .removeClass('product-gallery__carousel-item--active')
                .eq(current)
                .addClass('product-gallery__carousel-item--active');
            var onscreen = carousel.find('.owl-item.active').length - 1;
            var start = carousel.find('.owl-item.active').first().index();
            var end = carousel.find('.owl-item.active').last().index();

            if (current > end) {
                carousel.data('owl.carousel').to(current, 100, true);
            }
            if (current < start) {
                carousel.data('owl.carousel').to(current - onscreen, 100, true);
            }
        }
    };

    $(function() {
        $('.product').each(function () {
            var gallery = $(this).find('.product-gallery');

            if (gallery.length > 0) {
                initProductGallery(gallery[0], $(this).data('layout'));
            }
        });
    });


    /*
    // Checkout payment methods
    */
    $(function () {
        $('[name="checkout_payment_method"]').on('change', function () {
            var currentItem = $(this).closest('.payment-methods__item');

            $(this).closest('.payment-methods__list').find('.payment-methods__item').each(function (i, element) {
                var links = $(element);
                var linksContent = links.find('.payment-methods__item-container');

                if (element !== currentItem[0]) {
                    var startHeight = linksContent.height();

                    linksContent.css('height', startHeight + 'px');
                    links.removeClass('payment-methods__item--active');

                    linksContent.css('height', '');
                } else {
                    var startHeight = linksContent.height();

                    links.addClass('payment-methods__item--active');

                    var endHeight = linksContent.height();

                    linksContent.css('height', startHeight + 'px');
                    linksContent.css('height', endHeight + 'px');
                }
            });
        });

        $('.payment-methods__item-container').on('transitionend', function (event) {
            if (event.originalEvent.propertyName === 'height') {
                $(this).css('height', '');
            }
        });
    });


    /*
    // collapse
    */
    $(function () {
        $('[data-collapse]').each(function (i, element) {
            var collapse = element;
            var openedClass = $(element).data('collapse-opened-class');

            $('[data-collapse-trigger]', collapse).on('click', function () {
                var item = $(this).closest('[data-collapse-item]');
                var content = item.children('[data-collapse-content]');
                var itemParents = item.parents();

                itemParents.slice(0, itemParents.index(collapse) + 1).filter('[data-collapse-item]').css('height', '');

                if (item.is('.' + openedClass)) {
                    var startHeight = content.height();

                    content.css('height', startHeight + 'px');
                    item.removeClass(openedClass);

                    content.css('height', '');
                } else {
                    var startHeight = content.height();

                    item.addClass(openedClass);

                    var endHeight = content.height();

                    content.css('height', startHeight + 'px');
                    content.css('height', endHeight + 'px');
                }
            });

            $('[data-collapse-content]', collapse).on('transitionend', function (event) {
                if (event.originalEvent.propertyName === 'height') {
                    $(this).css('height', '');
                }
            });
        });
    });


    /*
    // price filter
    */
    $(function () {
        $('.filter-price').each(function (i, element) {
            var min = $(element).data('min');
            var max = $(element).data('max');
            var from = $(element).data('from');
            var to = $(element).data('to');
            var slider = element.querySelector('.filter-price__slider');

            noUiSlider.create(slider, {
                start: [from, to],
                connect: true,
                direction: isRTL() ? 'rtl' : 'ltr',
				step: 1000,
                range: {
                    'min': min,
                    'max': max
				},
				format: {
					to: function (value) {
						return value.toLocaleString();
					},
					from: function (value) {
						return value.toLocaleString();
					}
				}
            });

            var titleValues = [
                $(element).find('.filter-price__min-value')[0],
                $(element).find('.filter-price__max-value')[0]
            ];

            slider.noUiSlider.on('update', function (values, handle) {
                titleValues[handle].innerHTML = values[handle];
            });
        });
    });


    /*
    // mobilemenu
    */
    $(function () {
        var body = $('body');
        var mobilemenu = $('.mobilemenu');

        if (mobilemenu.length) {
            var open = function() {
                var bodyWidth = body.width();
                body.css('overflow', 'hidden');
                body.css('paddingRight', (body.width() - bodyWidth) + 'px');

                mobilemenu.addClass('mobilemenu--open');
            };
            var close = function() {
                body.css('overflow', 'auto');
                body.css('paddingRight', '');

                mobilemenu.removeClass('mobilemenu--open');
            };


            $('.mobile-header__menu-button').on('click', function() {
                open();
            });
            $('.mobilemenu__backdrop, .mobilemenu__close').on('click', function() {
                close();
            });
        }
    });


    /*
    // tooltips
    */
    $(function () {
        $('[data-toggle="tooltip"]').tooltip({trigger: 'hover'});
    });


    /*
    // layout switcher
    */
    $(function () {
        $('.layout-switcher__button').on('click', function() {
            var layoutSwitcher = $(this).closest('.layout-switcher');
            var productsView = $(this).closest('.products-view');
            var productsList = productsView.find('.products-list');

            layoutSwitcher.find('.layout-switcher__button').removeClass('layout-switcher__button--active');
            $(this).addClass('layout-switcher__button--active');

            productsList.attr('data-layout', $(this).attr('data-layout'));
            productsList.attr('data-with-features', $(this).attr('data-with-features'));
        });
    });
	
	/*
    // initialize light gallery
    */
	$(function () {
		$('.product-gallery__featured').lightGallery({
			selector: 'a',
			loop: false,
			mousewheel: false,
			download: false,
			actualSize: false
		});
	});
	
})(jQuery);
