var CopyPastePlugin =
{
  CopyPasteReader: function(vName)
  {
      var voidName = UTF8ToString(vName);
      var inp =document.createElement('input');
        document.body.appendChild(inp)
        inp.value =voidName;
        inp.select();
        document.execCommand('copy');
       inp.remove(); 
  }
};
mergeInto(LibraryManager.library, CopyPastePlugin);