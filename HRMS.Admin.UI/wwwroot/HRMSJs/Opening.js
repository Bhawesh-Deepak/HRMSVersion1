function GetOpeningDetails() {
    GetCustomRecord("/CurrentOpening/GetOpeningDetails", "divHRMS")
}

$(document).ready(function () {
    GetOpeningDetails();
})

function AddOpening() {
    NewCustomRecord("/CurrentOpening/CreateOpening", "Create Current Openings")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/CurrentOpening/GetOpeningDetails", "/CurrentOpening/DeleteOpening", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/CurrentOpening/CreateOpening", "Create Current Openings", "Update Current Openings");
}

function DisplayIFramePDF(pdfDetails) {
    $("#displayPDFFrame").attr("src", pdfDetails);
    $("#divModalPop").modal('show');
}

$(function () {
    $("#PdfFile").change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                showInCanvas(e.target.result);
            }
            reader.readAsDataURL(this.files[0]);
        }
    });

    function convertDataURIToBinary(dataURI) {
        var BASE64_MARKER = ';base64,';
        var base64Index = dataURI.indexOf(BASE64_MARKER) + BASE64_MARKER.length;
        var base64 = dataURI.substring(base64Index);
        var raw = window.atob(base64);
        var rawLength = raw.length;
        var array = new Uint8Array(new ArrayBuffer(rawLength));

        for (i = 0; i < rawLength; i++) {
            array[i] = raw.charCodeAt(i);
        }
        return array;
    }

    function showInCanvas(url) {
        'use strict';
        var pdfAsArray = convertDataURIToBinary(url);
        pdfjsLib.getDocument(pdfAsArray).then(function (pdf) {
            pdf.getPage(1).then(function (page) {
                var scale = 1.5;
                var viewport = page.getViewport(scale);
                var canvas = document.getElementById('the-canvas');
                var context = canvas.getContext('2d');
                canvas.height = viewport.height;
                canvas.width = viewport.width;
                var renderContext = {
                    canvasContext: context,
                    viewport: viewport
                };
                page.render(renderContext);
            });
        });
    }
});