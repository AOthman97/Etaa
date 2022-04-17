// Method to show the pop-up, it takes two arguments the url and the title, Originally made for the AddOrEdit for the family
// member page. Currently not working with it and replaced it with the Create for the family member page
ShowPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").html('show');
        }
    })
}
