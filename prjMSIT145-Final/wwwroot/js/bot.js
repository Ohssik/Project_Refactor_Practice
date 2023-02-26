﻿const msgDiv = document.querySelector(".msgDivProblem"); //對話框
const userImg = document.querySelector("#userImgProblem"); //客戶頭像
const txt = document.querySelector("#txtProblem"); //輸入框
const btnSubmitProblem = document.querySelector("#btnSubmitProblem"); //送出
const tr = document.getElementsByName("trProblem");
const resultDiv = document.querySelector("#resultDivProblem");
let azureLogo = document.querySelector("#azureLogo"); //按鈕
let logoDiv = document.querySelector("#logoDiv");
let timer = null; //時間變數

let msgProblem = "";
// 點選常見問題
tr.forEach(function (msg) {
    msg.addEventListener("click", function () {
        msgProblem = this.firstElementChild.innerHTML;
        txt.value = msgProblem;
    });
});

//按下送出鈕
btnSubmitProblem.addEventListener("click", async function () {
    //使用者頭像ajax
    const response = await fetch("/Problem/PUserImg");
    const img = await response.json();
    //問題ajax
    const request = await fetch(`/Problem/PAnswer?keyword=${txt.value}`);
    const data = await request.json();


    if (txt.value === "") {
        alert("請輸入內容")

        return false;
    }
    let fragForUser = document.createDocumentFragment();
    let fragForService = document.createDocumentFragment();

    fragForService = `<div class="displayProblem serviceDiv">
                     <div class="imgDiv"><img src="../images/Problem/dailyLogo.png" alt="ServiceImg" /></div>
                    <div class="serviceMsg">
                               ${data.answer}
                    </div>
                    </div>`;

    //判斷使用者有無頭像
    if (img.userImg !== "") {
        fragForUser = `<div class="displayProblem userDiv">
                                    <div class="userMsg">${txt.value}</div>
                                        <div class="imgDiv" id='userImgProblem'><img src="../images/Customer/Member/${img.userImg}" alt="SessionImg"/></div>
                                    </div > `;
    }
    else {
        fragForUser = `<div class="displayProblem userDiv">
                                    <div class="userMsg">${txt.value}</div>
                                        <div class="imgDiv" id='userImg' style='width:80px;height:75px'><img src="../images/Problem/chicken.jpg" alt="UserImg"/></div>
                                    </div > `;
    }
    resultDiv.innerHTML += fragForUser;
    resultDiv.innerHTML += fragForService;
    msgDiv.scrollTop = msgDiv.scrollHeight;

    txt.value = "";
});


let hight = document.documentElement.scrollHeight;  //需要顯示得總長
document.addEventListener("scroll", function () {
    clearTimeout(timer);

    let top = document.documentElement.scrollTop; //頁面目前所在高度

    const azureClass = azureLogo.classList.contains("walk");  //檢查是否有'walk'的class

    //停止捲動事件一秒後移除動畫class
    timer = setTimeout(function () {
        azureLogo.classList.remove("walk");
        logoDiv.classList.remove("walk");
    }, 500);

    //判斷有無此class
    if (!azureClass) {
        azureLogo.classList.add('walk');

        setTimeout(function () {
            logoDiv.classList.add("walk");
        }, 500)
    }

    //接近頁面最底時修改定位
    if ((hight - top) <= 1100 ) {
         azureLogo.setAttribute("style", "bottom:8%;");
        logoDiv.setAttribute("style", "bottom:8%");
        document.querySelector(".logo").setAttribute("style", "bottom:20%;")
        document.querySelector(".azure").setAttribute("style", "bottom:20%;")
    }
    else {
        azureLogo.setAttribute("style", "bottom:5%");
        logoDiv.setAttribute("style", "bottom:5%");
        document.querySelector(".logo").setAttribute("style", "bottom:15%;")
        document.querySelector(".azure").setAttribute("style", "bottom:15%;")
    }
})

if (hight <= document.documentElement.clientHeight) {
    azureLogo.setAttribute("style", "bottom:8%;");
    logoDiv.setAttribute("style", "bottom:8%;");
    document.querySelector(".logo").setAttribute("style", "bottom:20%;")
    document.querySelector(".azure").setAttribute("style", "bottom:20%;")
}