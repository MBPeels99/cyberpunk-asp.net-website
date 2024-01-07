function validateForm() {
    // Helper function to show error
    function showError(message) {
        Swal.fire('Validation Error', message, 'error');
        return false;
    }

    // Helper function to validate date
    function isValidDate(dateString) {
        var date = new Date(dateString);
        return date instanceof Date && !isNaN(date);
    }

    var fullName = document.getElementById('txtFullName').value;
    var placeOfBirth = document.getElementById('txtPlaceOfBirth').value;
    var status = document.getElementById('txtStatus').value;
    var gender = document.getElementById('ddlGender').value;
    var hairColor = document.getElementById('txtHairColor').value;
    var eyeColor = document.getElementById('txtEyeColor').value;
    var knownFor = document.getElementById('txtKnownFor').value;
    var background = document.getElementById('txtBackground').value;
    var partner = document.getElementById('txtPartner').value;
    var voicedBy = document.getElementById('txtVoicedBy').value;
    var dateOfBirth = document.getElementById('txtDateOfBirth').value;
    var dateOfDeath = document.getElementById('txtDateOfDeath').value;
    var occupation = document.getElementById('Occupation').value;
    var affiliation = document.getElementById('Affiliation').value;

    if (!fullName) return showError('Full Name is required');
    if (!placeOfBirth) return showError('Place of Birth is required');
    if (!status) return showError('Status is required');
    if (!gender) return showError('Gender is required');
    if (!hairColor) return showError('Hair Color is required');
    if (!eyeColor) return showError('Eye Color is required');
    if (!knownFor) return showError('Known For is required');
    if (!background) return showError('Background is required');
    if (!partner) return showError('Partner is required');
    if (!voicedBy) return showError('Voiced By is required');
    if (!isValidDate(dateOfBirth)) return showError('Invalid Date of Birth');
    if (dateOfDeath && !isValidDate(dateOfDeath)) return showError('Invalid Date of Death');
    if (occupation === "0") return showError('Occupation is required');
    if (affiliation === "0") return showError('Affiliation is required');

    // If all validations pass
    return true;
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

function addAffiliation(AffiliationName, AffiliationDescription) {
    $.ajax({
        type: "POST",
        url: "AddFigures.aspx/AddAffiliation",
        data: JSON.stringify({ AffiliationName: AffiliationName, AffiliationDescription: AffiliationDescription }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            alert("Affiliation added successfully");
            // Additional success handling
        },
        error: function (error) {
            alert("Error adding affiliation");
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

function promptForNewAffiliation() {
    Swal.fire({
        title: 'Enter New Affiliation',
        html:
            '<input id="swal-input1" class="swal2-input" placeholder="Affiliation Name">' +
            '<textarea id="swal-input2" class="swal2-textarea" placeholder="Affiliation Description"></textarea>',
        showCancelButton: true,
        focusConfirm: false,
        preConfirm: () => {
            const name = document.getElementById('swal-input1').value;
            const description = document.getElementById('swal-input2').value;
            if (!name || !description) {
                Swal.showValidationMessage("Both fields are required");
            }
            return { name: name, description: description }
        }
    }).then((result) => {
        if (result.value) {
            addAffiliation(result.value.name, result.value.description);
        }
    });
}