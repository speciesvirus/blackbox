

function notifySuccess(amessage) {
    $.notify({
        title: "Success : ",
        message: amessage
    }, {type : 'success'});

}


function notifyInfo(amessage) {
    $.notify({
        title: "Infomation : ",
        message: amessage
    }, { type: 'info' });
}

function notifyWarning(amessage) {
    $.notify({
        title: "Warning : ",
        message: amessage
    }, { type: 'warning' });
}


function notifyError(amessage) {
    $.notify({
        title: "Error : ",
        message: amessage
    }, { type: 'danger' });
}