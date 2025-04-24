// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function CheckWorker(workerId, srcUrl, supervisorId) {
    $.ajax({
        url: srcUrl,
        data: { id: workerId },
        success: function (result) {
            if (result) {
                const newDiv = document.createElement("div");
                newDiv.id = "Supervisor";
                const target = document.getElementById("HeadPractitioner");
                const parent = document.getElementById("Workers");
                parent.insertBefore(newDiv, target);
                window.$("#Supervisor").html(result);
                const select = document.getElementById("SupervisorSelect");
                if (supervisorId > 0 && supervisorId !== undefined) {
                    select.value = supervisorId;
                    const selectInfo = document.getElementById("SupervisorInfo");
                    selectInfo.classList.remove("val-error-text");
                    select.classList.remove("input-validation-error");
                } else {
                    select.value = -1;
                }
            } else {
                const elementExists = !!document.getElementById("Supervisor");
                if (elementExists) {
                    document.getElementById("Supervisor").remove();
                }
            }
        }
    });
};

function CheckBodyLocalization(bodyLocalization, srcUrl, diagnosisId) {
    $.ajax({
        url: srcUrl,
        data: { location: bodyLocalization },
        success: function (result) {
            if (result) {
                const elementExists = !!window.$("#Diagnosis");
                if (elementExists) {
                    window.$("#Diagnosis").remove();
                }
                const target = window.$("#BodyLocalization");
                window.$(target).after(result);
                const select = window.$("#DiagnosisSelect");
                if (diagnosisId >= 1) {
                    window.$(select).val(diagnosisId);
                    window.$(select).removeClass("input-validation-error");
                    window.$("#DiagnosisInfo").removeClass("val-error-text");
                }
            } else {
                const elementExists = !!window.$("#Diagnosis");
                if (elementExists) {
                    window.$("#Diagnosis").remove();
                }
            }
        }
    });
};

function CheckValue(workerId) {
    const selectInfo = document.getElementById("SupervisorInfo");
    const select = document.getElementById("SupervisorSelect");
    if (workerId < 1) {
        selectInfo.classList.add("val-error-text");
        select.classList.add("input-validation-error");
    } else {
        selectInfo.classList.remove("val-error-text");
        select.classList.remove("input-validation-error");
    }
}

function CheckDiagnosis(diagnosisId) {
    const selectInfo = window.$("#DiagnosisInfo");
    const select = window.$("#DiagnosisSelect");
    if (diagnosisId < 1) {
        window.$(selectInfo).addClass("val-error-text");
        window.$(select).addClass("input-validation-error");
    } else {
        window.$(selectInfo).removeClass("val-error-text");
        window.$(select).removeClass("input-validation-error");
    }
}

function AvailabilityOnLoad() {
    const elements = window.$(".form-group");
    for (let i = 0; i < elements.length - 1; i++) {
        const day = elements[i];
        const state = $(day).find(".day")[0].checked;
        const dayName = $(day).find(".day")[0].name;
        if (state === false) {
            const inputs = $(day).find(`input[name^="${dayName}"]`);
            inputs.splice(0, 1);
            for (let j = 0; j < inputs.length; j++) {
                inputs[j].disabled = true;
            }
        }
    }
}

function CheckInputState(event) {
    const element = $(event)[0];
    const state = element.checked;
    const dayName = element.name;
    const inputs = $(element).parent().parent().find(`input[name^="${dayName}"]`);
    inputs.splice(0, 1);
    if (state) {
        for (let b = 0; b < inputs.length; b++) {
            if (inputs[b].disabled === true) {
                inputs[b].disabled = false;
            }
        }
    } else {
        for (let j = 0; j < inputs.length; j++) {
            if (inputs[j].disabled === false) {
                inputs[j].disabled = true;
                inputs[j].value = "";
            }
        }
    }
}

function DeleteTreatmentPatient(button, srcUrl, treatmentId) {
    $.ajax({
        url: srcUrl,
        data: { id: treatmentId },
        success: function (result) {
            if (result) {
                $(button).closest(".view-row").remove();
            } else {
                $(button).parent().after("<p class=\"text-danger mt-1\">Een behandeling mag niet verwijdert worden een dag nadat die is aangemaakt.</p>");
            }
        }
    });
}

function CalculateTime(startDateTime, srcUrl, id) {
    if (id === 0) {
        id = $("#PatientId").val();
    }
    if (id > 0) {
        $.ajax({
            url: srcUrl,
            data: { dateTime: startDateTime, patientId: id },
            success: function (result) {
                if (typeof result === "boolean") {
                    $("#info").html("Patiënt is nog niet ingeschreven.");
                } else {
                    $("#EndTime").val(result);
                }
            }
        });
    }
}

function RemoveOldAppointments(element) {
    const checked = $(element).is(":checked");
    if (checked) {
        $('*[id*=OldAppointment]').each(function () {
            $(this).hide();
        });
    } else {
        $('*[id*=OldAppointment]').each(function () {
            $(this).show();
        });
    }
}

function FilterPatients(value) {
    $('table > tbody > tr').each(function () {
        let found = false;
        $(this).find("td").each(function () {
            if ($(this).text().toUpperCase().indexOf(value.toUpperCase()) >= 0) {
                found = true;
            }
        });
        if (found) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
}

function CancelAppointment(element, srcUrl, appointmentId) {
    $.ajax({
        url: srcUrl,
        data: { appointmentId: appointmentId },
        success: function (result) {
            if (result) {
                $(element).closest("tr").remove();
            } else {
                $(element).after("<p class=\"text-danger mt-1\">Een afspraak mag niet geannuleerd binnen 24 uur tot de dag dat deze is.</p>");
            }
        }
    });
}