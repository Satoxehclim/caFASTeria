// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.getElementById('fotoProducto').addEventListener('change', function (event) {
    const file = event.target.files[0];
    const preview = document.getElementById('file-preview');
    preview.innerHTML = ''; // Limpiar cualquier vista previa anterior

    if (file && (file.type === 'image/jpeg' || file.type === 'image/png')) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const img = document.createElement('img');
            img.src = e.target.result;
            preview.appendChild(img);
        };
        reader.readAsDataURL(file);
    } else {
        preview.textContent = 'Por favor selecciona un archivo JPG o PNG válido.';
    }
});

var textarea = document.getElementById('CrearDescripcionProducto');
var charCount = document.getElementById('CrearDescripcionProductocharCount');

textarea.addEventListener('input', function () {
    var remaining = 250 - textarea.value.length;
    charCount.textContent = remaining + ' caracteres restantes';
});