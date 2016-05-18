/**
 * 作者：胡锦柢
 * 用途：适用于移动端的angular迷你版，压缩大小：5.50k，gzip大小：1.84k
 * 依赖：纯原生js，无任何依赖
 * 概述：蜂飞医技(蓝海之略临床医疗技术服务平台移动端)插件
 * 支持：原生js调用或者require.js模块化
 * 调用：
 * 			一般用法
 * 				var angular = new hjd_angular(); 
 * 				angular.set('info', info);
 * 				angular.get('info');
 * 			require.js用法
 * 				define(['zepto', 'am-angular'], function($, angular) {
 * 					angular.set('info', info)
 * 					angular.get('info')
 * 				})
 * 用法：
 * 			 1. 双向绑定【am-bind】
 * 					<input am-bind="info.user.name"/>
 * 			 2. 单向绑定【am-single】
 * 					{{info.user.age>=20?成年:未成年}}
 * 					<input am-single="info.user.age>=20?成年:未成年"/>
 *  		 3. 绑定列表数据【am-repeat】
 * 					<ul am-repeat="info.users">
 *						<li>姓名：{{name}}，年龄：{{age}}</li>
 *					</ul>
 * 			 4. 根据字段值隐藏元素【am-hide】
 * 					<span am-hide="info.user.name" am-single="info.user.name"></span>
 * 			 5. 自动化的图片懒加载【am-src】，但是必须设置图片高度
 * 					<img height='75' am-src="{{info.src}}" />
 * */
function angular_mobile(){var repeat,o;return window.scope={},window.scope.elements=[],window.scope.events=[],repeat={},this._is_array=function(a){return"object"==typeof a&&null!=a&&void 0!=a&&"function"==typeof a.push?!0:!1},this._get_value=function(attr,e,isrepeat){var result,exp,exps,length,repeatKey,index,i,max;if(isrepeat){for(exps=attr.split("."),length=exps.length,repeatKey=exps.join(""),index=repeat[repeatKey],void 0==index||null==index?(repeat[repeatKey]=0,index=0):(repeat[repeatKey]=repeat[repeatKey]+1,index+=1),exp="window.scope",i=0;i<exps.length;i++)if(i<exps.length-2)exp+="."+exps[i];else if(i==exps.length-2){if(exp+="."+exps[i],eval("result="+exp),!this._is_array(result)){exp+="."+exps[i+1];break}}else i==exps.length-1&&(exp+="["+index+"]."+exps[i]);eval("result="+exp)}else exp="window.scope."+attr,eval("result="+exp);return e&&(max=e.getAttribute("maxlength"),max&&result&&(result=result.substr(0,parseInt(max)))),result||""},this._set_value=function(attr,value,e){var max,exp="window.scope."+attr;value=value.replace(/\n/gi,""),e&&(max=e.getAttribute("maxlength"),max&&value&&(value=value.substr(0,parseInt(max)))),eval(exp+"='"+value+"'")},this._add_event=function(a,b,c){var d=a[b];a[b]="function"!=typeof d?function(){c(a)}:function(){c(a),d.apply(this,arguments)}},this._invoke_expression=function(a,b,c){var d,f,g,h,i,j,k,e=this;return a.indexOf("?")>=0?(f=function(a){return c?c[a]:e._get_value(a).toString()},g=a.split("?"),h=null,i=g[1].split(":")[0],j=g[1].split(":")[1],g[0].indexOf("==")>=0&&null==h&&(k=g[0].split("=="),h=f(k[0])==k[1]?i:j),g[0].indexOf("!=")>=0&&null==h&&(k=g[0].split("!="),h=f(k[0])!=k[1]?i:j),g[0].indexOf(">=")>=0&&null==h&&(k=g[0].split(">="),h=f(k[0])>=k[1]?i:j),g[0].indexOf("<=")>=0&&null==h&&(k=g[0].split("<="),h=f(k[0])<=k[1]?i:j),g[0].indexOf(">")>=0&&null==h&&(k=g[0].split(">"),h=f(k[0])>k[1]?i:j),g[0].indexOf("<")>=0&&null==h&&(k=g[0].split("<"),h=f(k[0])<k[1]?i:j),d=h||""):d=e._get_value(a,b),d},this._invoke_value=function(a){for(var d,e,f,g,h,b=a,c=b.indexOf("{{");c>=0;)d=b.indexOf("}}",c+2),e=b.substring(c+2,d),f=e.length+4,g=this._invoke_expression(e),h=b.substring(c,d+2),b=b.replace(h,g),c=b.indexOf("{{",d+2-(f-g.length));return b},this._bind_double=function(){var b,c,d,e,f,a=this;for(b=0;b<document.all.length;b++)c=document.all[b],d=c.getAttribute("am-bind"),d&&(e=this._get_value(d,c),f=c.toString(),"[object HTMLInputElement]"==f?(c.value=e,this._add_event(c,"onchange",function(b){var c=b,d=c.getAttribute("am-bind"),e=c.value;a._set_value(d,e,c)})):"[object HTMLTextAreaElement]"==f?(c.innerHTML=e,this._add_event(c,"onchange",function(b){var c=b,d=c.getAttribute("am-bind"),e=c.value;a._set_value(d,e,c)})):"[object HTMLSelectElement]"==f?(c.value=e,this._add_event(c,"onchange",function(b){var c=b,d=c.getAttribute("am-bind"),e=c.value;a._set_value(d,e,c)})):c.innerHTML=e)},this._bind_single=function(a){var c,d,e,f,g,h,i,j,k,l,b=a||document.body.childNodes;for(c=0;c<b.length;c++)if(d=b[c].nodeValue,e=b[c].innerHTML,f=b[c].nodeName,g=b[c].attributes,h=b[c].childNodes,"#comment"!=f&&"SCRIPT"!=f){if(d&&(b[c].nodeValue=this._invoke_value(d)),("OPTION"==f||"LI"==f)&&e&&(b[c].innerHTML=this._invoke_value(e)),g&&g.length>0)for(i=0;i<g.length;i++)j=g[i].nodeName||g[i].name,k=g[i].nodeValue,"am-single"==j?(b[c].value=this._invoke_expression(k,b[c]),b[c].innerHTML=this._invoke_expression(k,b[c])):k.indexOf("{{")>=0&&(g[i].nodeValue=this._invoke_value(k)),j.indexOf("{{")>=0&&(l=j.replace("{{","").replace("}}",""),l=this._get_value(l),l&&b[c].setAttribute(l,""));h.length>0&&this._bind_single(h)}},this._bind_repeat=function(){var a,b,c,d,e,f,g,h,i,j,k,l,m,n;for(a=0;a<document.all.length;a++)if(b=document.all[a],c=b.getAttribute("am-repeat")){if(d=this._get_value(c,null,!0),e=this._get_value(c+"_TEMPLATE"),e||(this._set_value(c+"_TEMPLATE",b.innerHTML),e=b.innerHTML),f="",g="",d&&d.length>0)for(h=0;h<d.length;h++){for(i=d[h],g=e,j=g.indexOf("{{"),k=-1;j>=0;)k=g.indexOf("}}",j),l=g.substring(j+2,k),m=g.substring(j,k+2),n="",n="$v"==l?i:"$index"==l?h+1:l.indexOf("?")>=0?this._invoke_expression(l,null,i):i[l]||i[l]===!1||0===i[l]?i[l].toString():"",n?(g=g.replace(m,n),j=g.indexOf("{{")):j=g.indexOf("{{",k);f+=g}b.innerHTML=f,b.style.display=""}},this._Screen={ViewHeight:function(){var a=document,b="BackCompat"==a.compatMode?a.body:a.documentElement;return b.clientHeight},Height:function(){var a=document,b=a.body,c=a.documentElement,d="BackCompat"==a.compatMode?b:a.documentElement;return Math.max(c.scrollHeight,b.scrollHeight,d.clientHeight)},Top:function(a){var b=a.offsetTop;return null!=a.offsetParent&&(b+=this.Top(a.offsetParent)),b},ScrollBar:function(){var a;return window.pageYOffset?a=window.pageYOffset:document.compatMode&&"BackCompat"!=document.compatMode?a=document.documentElement.scrollTop:document.body&&(a=document.body.scrollTop),a}},this._bind_src=function(){var d,e,f,g,a=this._Screen.ScrollBar(),b=this._Screen.ViewHeight()+a;for(this._Screen.Height(),d=document.getElementsByTagName("img"),e=0;e<d.length;e++)f=d[e].getAttribute("am-src"),f&&(g=this._Screen.Top(d[e]),b>g&&(d[e].setAttribute("src",f),d[e].setAttribute("am-src","")))},this._bind_hide=function(){var a,b,c,d;for(a=0;a<document.all.length;a++)b=document.all[a],c=b.getAttribute("am-hide"),c&&(d=null,d=-1==c.indexOf("!")?!!this._get_value(c).toString():!this._get_value(c.substr(1)).toString(),b.style.display=d?"":"none")},o=this,{get:function(a){return window.scope[a]},set:function(a,b,c){var d=window.setTimeout(function(){if(document.body){try{window.scope[a]=b,o._bind_repeat(),o._bind_hide(),o._bind_single(),o._bind_double(),o._bind_src(),o._add_event(window,"onscroll",function(){o._bind_src()});for(k in repeat)repeat[k]=null;c&&c()}catch(e){console.error(e.stack)}window.clearTimeout(d)}},50)}}}"function"==typeof define&&define.amd&&define(function(){return new angular_mobile});