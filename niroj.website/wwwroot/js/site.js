$(document).ready(function () {
    $('#navigation').on('click', '.nav-item', setSidebarLinksActiveBasedOnLinksClicked);
});

function setSidebarLinksActiveBasedOnLinksClicked() {
    $('#navigation').find('.nav-link').removeClass('active');
    $(this).find('.nav-link').addClass('active');
}

async function sendMessage() {
    event.preventDefault();
    let errorContainer = $('#msgSubmit');
    let captchaResponse = grecaptcha.getResponse();
    if (captchaResponse == undefined || captchaResponse == '') {
        addClass(errorContainer, 'text-danger');
        errorContainer.text('Please complete recaptcha verification.');
        return;
    }
    $(this).prop('disabled', 'true');
    let form = $(this).closest('form');
    let data = {
        Recaptcha: grecaptcha.getResponse(),
        Email: $('.email').val(),
        Name: $('.name').val(),
        Comment: $('.message').val()

    };
    const response = await fetch(`/contact/send-message`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    try {
        var jsonData = await response.json();
        if (isErrorResponse(jsonData)) {
            addClass(errorContainer, 'text-danger');
            errorContainer.text(jsonData.error);
        }
        else {
            addClass(errorContainer, 'text-info');
            errorContainer.text('Message Sent Successfully.');
            clearFormValues(form);
        }
    } catch (e) {
        addClass(errorContainer, 'text-danger');
        errorContainer.text('Failed to send message. Please try again later.');
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
        $(this).val('');
    });
}

function isErrorResponse(response) {
    if (response.error) {
        return true;
    }
    return false;
}