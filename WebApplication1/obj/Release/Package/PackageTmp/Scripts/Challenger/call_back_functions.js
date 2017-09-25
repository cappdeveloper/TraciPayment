

var callback_Functions = new function () {

    return {
        onSuccess: function (data) {
            if (data.redirectTo) {
                window.location.href = data.redirectTo;
            }
            else {
                switch (data.NotifyType) {
                    case 0: $("#alert-container").html(data.Html);
                        
                        break;
                }

                
            }
        },
        onBeforeSend: function () {
            commonApp.disableFormButton();
            commonApp.showLoadingImage();
        },
        onComplete: function () {
            commonApp.enableFormButton();
            commonApp.hideLoadingImage();
        },
        onError: function (xhr, textStatus, errorThrown) {
            commonApp.enableFormButton();
            commonApp.hideLoadingImage();
            alert(xhr + textStatus + errorThrown);
        }
    };
}