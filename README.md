# BaiduPCS
## 百度网盘上传下载
<br>
<p>使用Visual C# 2010开发</p>
<br>
## 关于dv参数的计算
<p>2017.3.8后，百度登录时增加了dv参数，此参数是通过<a href="https://passport.baidu.com/static/passpc-base/js/dv/3.min.js">https://passport.baidu.com/static/passpc-base/js/dv/3.min.js</a>这个脚本对用户的信息进行计算的，但最终结果如何并不影响登录，可以使用某一次登录时的值作为固定dv值。</p>
<br>
## 关于301、302返回码
<br>
<p>百度登录过程中，会遇到301、302返回码，示意URL需要进行跳转处理，代码中有对此进行相应的处理过程，而且对于POST请求返回的301、302，会转换成GET请求进行跳转</p>
