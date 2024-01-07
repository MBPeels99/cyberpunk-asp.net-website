function validateForm() {
    var fullName = document.getElementById("txtFullName").value;

    if (fullName === "") {
        swal("Validation Error", "Full Name is required", "error");
        return false;
    }

    // Add more validations as needed

    return true; // Only return true if all validations pass
}
function addOccupation(occupationName) {
    $.ajax({
        type: "POST",
        url: "AddFigures.aspx/AddOccupation",
        data: JSON.stringify({ occupationName: occupationName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(response) {
            alert("Occupation added successfully");
            // Additional success handling
        },
        error: function(error) {
            alert("Error adding occupation");
            // Error handling
        }
    });
}


function promptForNewOccupation() {
    Swal.fire({
        title: 'Enter new occupation',
        input: 'text',
        inputLabel: 'Occupation',
        showCancelButton: true,
        inputValidator: (value) => {
            if (!value) {
                return 'You need to write something!'
            }
        }
    }).then((result) => {
        if (result.value) {
            addOccupation(result.value);
        }
    });
}