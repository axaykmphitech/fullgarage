var DownloadFilePlugin = {
  DownloadFile : function(array, size, fileNamePtr)
  {
    var fileName = UTF8ToString(fileNamePtr);
    var bytes = new Uint8Array(size);
    for (var i = 0; i < size; i++)
    {
       bytes[i] = HEAPU8[array + i];
    }
    var blob = new Blob([bytes]);
    var a = document.createElement("a"),
    url = URL.createObjectURL(blob);
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    setTimeout(function() {
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
    }, 1000);
  }
};
mergeInto(LibraryManager.library, DownloadFilePlugin);
