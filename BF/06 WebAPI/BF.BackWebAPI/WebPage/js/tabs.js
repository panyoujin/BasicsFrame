var index = 0;
function addTab(){
  index++;
  $('#tt').tabs('add',{
    title:'New Tab ' + index,
    content:'Tab Body ' + index,
    iconCls:'icon-save',
    closable:true,
    tools:[{
      iconCls:'icon-mini-refresh',
      handler:function(){
        alert('refresh');
      }
    }]
  });
}
function getSelected(){
  var tab = $('#tt').tabs('getSelected');
  alert('Selected: '+tab.panel('options').title);
  }
function update(){
  index++;
  var tab = $('#tt').tabs('getSelected');
  $('#tt').tabs('update', {
    tab: tab,
    options:{
      title:'new title'+index,
      iconCls:'icon-save'
    }
  });
}