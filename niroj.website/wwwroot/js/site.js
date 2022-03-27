$(document).ready(function () {
    $('#navigation').on('click', '.nav-item', setSidebarLinksActiveBasedOnLinksClicked);
    $('#btn-contact-submit').on('click', sendMessage);
    $('#subscribe').on('click', subscribeToNewsletter);
});

function lazyLoadImages() {
    const observer = lozad();
    observer.observe();
}
function setSidebarLinksActiveBasedOnLinksClicked() {
    $('#navigation').find('.nav-link').removeClass('active');
    $(this).find('.nav-link').addClass('active');
}

function ShowToastMessage(type, message) {
    if (type == 'info') {
        toastr.info(message);
    }
    else if (type == 'error') {
        toastr.error(message);
    }
    else {
        toastr.success(message);
    }
}

async function subscribeToNewsletter() {
    event.preventDefault();
    let email = $('#email').val();
    if (!email) {
        toastr.info('Please enter your email.', 'Information!!');
        return;
    }

    $(this).prop('disabled', 'true');
    let form = $(this).closest('form');
    let data = {
        Email: email
    };
    const response = await fetch(`/blogs/subscribe`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    try {
        var jsonData = await response.json();
        if (isErrorResponse(jsonData)) {
            toastr.error(JSON.parse(jsonData).error)
        }
        else {
            toastr.success('Newsletter subscribed successfully.', 'Success!!')
        }
    } catch (e) {
        toastr.error('Failed to subscribe Newsletter. Please try again later.', 'Error!!');
    }
}

async function sendMessage() {
    event.preventDefault();
    let captchaResponse = grecaptcha.getResponse();
    if (captchaResponse == undefined || captchaResponse == '') {
        toastr.error('Please complete recaptcha verification.', 'Error!!');
        return;
    }
    $(this).prop('disabled', 'true');
    let form = $(this).closest('form');
    let data = {
        Recaptcha: grecaptcha.getResponse(),
        Email: $('.Email').val(),
        Name: $('.Name').val(),
        Comment: $('.Comment').val()
    };
    const response = await fetch(`/contact/send-message`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    try {
        var jsonData = await response.json();
        if (isErrorResponse(jsonData)) {
            toastr.error(JSON.parse(jsonData).error, 'Error!!')
        }
        else {
            toastr.success('Message Sent Successfully.', 'Success!!')
            clearFormValues(form);
        }
    } catch (e) {
        toastr.error('Failed to send message. Please try again later.', 'Error!!');
    }
    $(this).prop('disabled', 'false');
}

function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function addClass(container, className) {
    if (!container.hasClass(className)) {
        container.addClass(className);
    }
}

function clearFormValues(form) {
    form.find('input,textarea').each(function () {
        if ($(this).hasClass('button')) {
            return true;
        }
        $(this).val('');
    });
}

function isErrorResponse(response) {
    window.response = response;
    if (JSON.parse(response).error) {
        return true;
    }
    return false;
}