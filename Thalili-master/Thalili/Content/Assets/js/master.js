// Start Global Functions
document.addEventListener('click', (e) => {
    let menu = document.querySelector('.arrowMenu');
    if (menu != null && e.target.getAttribute('id') !== "arrowBtn" && e.target.getAttribute('id') !== "arrow") {
        menu.classList.remove(menu.getAttribute('data-Todisplay'));
    }
});
function isDigit(char) {
    let asciiOfChar = char.charCodeAt(0);
    if ((asciiOfChar >= 48 && asciiOfChar <= 57)) {
        return true;
    }
    return false;
}
function isChar(char) {
    let asciiOfChar = char.charCodeAt(0);
    if (asciiOfChar >= 1569 && asciiOfChar <= 1610) {
        return true;
    }
    if ((asciiOfChar >= 65 && asciiOfChar <= 90) || (asciiOfChar >= 97 && asciiOfChar <= 122)) {
        return true;
    }
    return false;
}
function isSpecial(char) {
    if (char === ' ' || char === '#' || char === '@' || char === '$' || char === '*' || char === "+") {
        return true;
    }
    return false;
}
function checkPasswordValidation(pass, small) {
    if (pass.length == 0) {
        small.style.visibility = "hidden";
    }
    else if (pass.length < 8) {
        small.textContent = "كلمة المرور يجب ان تكون اكبر من 8 احرف";
        small.style.color = "red";
        small.style.visibility = "visible";
    }
    else {
        let ischar = false, isdigit = false, isspecial = false, isnot = true;
        for (let it of pass) {
            if (isDigit(it)) {
                isdigit = true;
            }
            else if (isChar(it)) {
                ischar = true;
            }
            else if (isSpecial(it)) {
                isspecial = true;
            }
            else {
                isnot = false;
                small.textContent = "كلمة المرور تحتوي علي علامات غير مسموح بها";
                small.style.visibility = "visible";
            }
        }
        if (isnot === false) {
            small.style.color = "red";
            small.textContent = "كلمة المرور تحتوي علي علامات غير مسموح بها";
        }
        else {
            if (ischar && isdigit && isspecial) {
                small.textContent = "كلمة المرور قوية"
                small.style.color = "green";
                return true;
            }
            else {
                small.style.color = "red";
                small.textContent = "كلمة المرور يجب ان تحتوي علي احرف وارقام وعلامات (@,#,$,*, ,+)";
            }
        }
    }
    return false;
}
function checkPasswordEquality(pass1, pass2, small) {
    if (pass1.length === 0) {
        small.style.visibility = "hidden";
    }
    else if (pass1 !== pass2) {
        small.style.color = "red";
        small.textContent = "كلمة المرور غير متطابقة";
        small.style.visibility = "visible";
    }
    else {
        small.style.color = "green";
        small.textContent = "كلمتا المرور متطابقتان";
        small.style.visibility = "visible";
        return true;
    }
    return false;
}
// End Global Functions

// Start  Functions of Navbar
function arrow_clicked() {
    let menu = document.querySelector('.arrowMenu');
    let classname = menu.getAttribute('data-Todisplay');
    menu.classList.toggle(classname);
}
// End  Functions of Navbar

//Start Functions of Login Page 
function changeLogin(h3text, formaction, idofbutton, idofanother, backgroundclass, removeoldbackgroundclass) {
    let element = document.querySelector(".FormPage");
    let h3InElement = element.querySelector('h3');
    let forminElement = element.querySelector('form');
    h3InElement.textContent = h3text;
    forminElement.setAttribute('action', formaction);
    document.querySelector(idofbutton).classList.add("loginChangeBtnsActive");
    document.querySelector(idofanother).classList.remove("loginChangeBtnsActive");
    document.querySelector(".FormPage").classList.add(backgroundclass);
    document.querySelector(".FormPage").classList.remove(removeoldbackgroundclass);
    if (idofbutton === "#lab-owner-btn") {
        forminElement.querySelector('.aboutParagraph').textContent = "تسجيل الدخول كصاحب معمل مخصص لاصحاب المعامل لادارة التحاليل الخاصة بالمعمل ومتابعة الطلبات";
    }
    else {
        forminElement.querySelector('.aboutParagraph').textContent = "تسجيل الدخول كمستخدم عادي يتيح لك إمكانيات اختيار التحاليل والمعامل وطلب عميل لاخذ العينات اللازمة";
    }
}
function changeLoginUserButton() {
    changeLogin("مستخدم عادي", "/Login/LoginConfirm", "#user-btn", "#lab-owner-btn", "aLoginPageForUser", "aLoginPageForLabOwner");
}
function changeLoginLabOwnerButton() {
    changeLogin("صاحب معمل", "/Login/LabLogin", "#lab-owner-btn", "#user-btn", "aLoginPageForLabOwner", "aLoginPageForUser");
}
//End Functions of Login Page 


// Start check Password on Forms
let formofpage = document.forms[0];
if (formofpage != null) {
    let passwordFields = document.querySelectorAll('input[type="password"]');
    if (passwordFields.length > 0) {
        let pass; let valid1 = false; let valid2 = -1;
        if (passwordFields.length > 1) {

            passwordFields[0].addEventListener('input', (e) => {
                valid1 = checkPasswordValidation(e.target.value, formofpage.querySelector('input[type="password"] ~ small'));
                if (passwordFields[1] != null) {
                    valid2 = checkPasswordEquality(passwordFields[1].value, e.target.value, document.querySelectorAll('input[type="password"] ~ small')[1]);
                }
                pass = e.target.value;
            });
            passwordFields[1].addEventListener('input', (e) => {
                let small2 = document.querySelectorAll('input[type="password"] ~ small')[1];
                valid2 = checkPasswordEquality(e.target.value, pass, small2);
            });
            console.log(valid1 + "    " + valid2);
        }

        formofpage.onsubmit = (event) => {
            if (valid2 !== -1) {

                if (!(valid1 && valid2)) {
                    event.preventDefault();
                }
                else {
                    formofpage.submit();
                }
            }
            // else{
            //     if(!valid1){
            //         event.preventDefault();
            //     }
            //     else{
            //         formofpage.submit();
            //     }
            // }
        };
    }
}
// End check Password on Forms

// LabOwner Dashboard
function active_Element(theLi) {
    /*let ul = theLi.parentElement.parentElement;
    let ul_childs = ul.children;
    for (let i = 0; i < ul_childs.length - 1; i++) {
        if (ul_childs[i] === theLi.parentElement) {
            ul_childs[i].classList.add('active');
        }
        else {
            ul_childs[i].classList.remove('active');
        }
    }*/
}
function addorEditForm(target, h3Text, formaction, submitValue) {
    let section = document.getElementById(target);
    let h3section = section.querySelector('h3');
    h3section.textContent = h3Text;
    section.classList.remove('d-none');
    let form = section.querySelector('form');
    form.setAttribute('action', formaction);
    form.querySelector('input[type="submit"]').setAttribute('value', submitValue);
}
function addorEditThalil(theElement) {
    let type = theElement.getAttribute('data-buttonType');
    let target = theElement.getAttribute('data-parentTarget');
    if (type === 'add') {
        addorEditForm(target, "اضافة تحليل جديد", "addThalil", "إضافة");
        document.getElementById(target).querySelector('#thalilName').removeAttribute('readonly');
        document.getElementById(target).querySelector('#thalilName').setAttribute('value', '');
        document.getElementById(target).querySelectorAll('input[type="text"]')[1].setAttribute('value', '');
    }
    else if (type === 'edit') {
        addorEditForm(target, "تعديل التحليل الحالي", "editThalil", "تعديل");
        document.getElementById(target).querySelector('#thalilName').setAttribute('readonly', '');
        let nameofThalil = theElement.parentElement.parentElement.children[0].textContent;
        let priceofThalil = theElement.parentElement.parentElement.children[1].textContent;
        let newpriceofThalil = "";
        for (let i = 0; i < priceofThalil.length; i++) {
            if (isDigit(priceofThalil[i]) || priceofThalil[i] == '.') {
                newpriceofThalil += priceofThalil[i];
            }
        }
        document.getElementById(target).querySelector('#thalilName').setAttribute('value', nameofThalil);
        document.getElementById(target).querySelectorAll('input[type="text"]')[1].setAttribute('value', newpriceofThalil);
    }

}
function sendIdofDelete(TheParent) {
    let val = TheParent.parentElement.parentElement.querySelector('input[type="hidden"]').value;
    document.getElementById('YesDelete').setAttribute('value', val);
}

function uploadpdf(theButton) {
    const parent = theButton.parentElement;
    const inputs = parent.querySelectorAll("input[type='hidden']");
    const targetForm = document.querySelector('#Uploadpdf').querySelector('form');
    for (let i = 0; i < inputs.length; i++) {
        targetForm.querySelectorAll("input[type='hidden']")[i].value = inputs[i].value;
        targetForm.querySelectorAll("input[type='hidden']")[i].name = inputs[i].name;
    }
}

// Start Cart Page
function addorminusone(theButton) {
    const parent = theButton.parentElement.parentElement;
    const elem = parent.querySelector('.numberoford');
    let num = parseInt(elem.textContent);
    if (theButton.getAttribute('data-action') === 'plusBtn') {
        if (num < 10) {
            num++;
            elem.textContent = num;
        }
    }
    else {
        if (num > 1) {
            num--;
            elem.textContent = num;
        }
    }
    parent.querySelector('input[type="hidden"]').value = num;
    console.log(theButton.getAttribute('data-parentForm'));
    document.querySelector('#' + theButton.getAttribute('data-parentForm')).submit();
    /*document.getElementById(theButton.getAttribute('data-parentForm')).submit();*/
}
function deleteItemfromCart(theButton) {
    const parent = document.getElementById(theButton.getAttribute('data-parentID'));
    const inputs = parent.querySelectorAll("input[type='hidden']");
    const targetForm = document.querySelector('#DeleteCartfModal').querySelector('form');
    for (let i = 0; i < inputs.length - 1; i++) {
        targetForm.querySelectorAll("input[type='hidden']")[i].value = inputs[i].value;
        targetForm.querySelectorAll("input[type='hidden']")[i].name = inputs[i].name;
    }
}
// End Cart Page

function go_next_item(element) {
    const parent = element.parentElement.parentElement;
    const child_length = parent.children.length;
    parent.children[0].style.display = "none";
    parent.children[child_length - 2].insertAdjacentElement('afterend', parent.children[0]);
    parent.children[2].style.display = "block";
}

function senduserIdofDelete(element) {
    const parent = element.parentElement.parentElement;
    const id = parent.querySelector(".UserID").textContent;
    document.querySelector("#DeleteUser").querySelector("input[type='hidden']").value = id;
}
function sendAnalysisIdofDelete(element) {
    const parent = element.parentElement.parentElement;
    const id = parent.querySelector(".AnalysisID").textContent;
    document.querySelector("#DeleteAnalsyis").querySelector("input[type='hidden']").value = id;
}
function AcceptandRefuseOrder(element) {
    const parent = element.parentElement.parentElement.parentElement;
    const id = parent.querySelector(".LabId").textContent;
    document.querySelector("#AcceptLabOrder").querySelector("input[type='hidden']").value = id;
    document.querySelector("#RefuseLabOrder").querySelector("input[type='hidden']").value = id;
}
function clearChangeImage(element) {
    const btn = element.parentElement.querySelector("button");
    if (element.value != null) {
        btn.classList.remove("d-none");
    }
}
function removeImage(element) {
    element.parentElement.querySelector("input[type='file']").value = null;
    element.parentElement.querySelector("button").classList.add("d-none");
}
function RefuseOrderfun(element) {
    const parent = element.parentElement.parentElement.parentElement;
    const id = parent.children[0].textContent;
    document.querySelector("#RefuseOrder").querySelector("input[type='hidden']").value = id;
}
function LabAvailable(element) {
    const state = element.getAttribute("data-stat");
    const child = element.querySelector(".toggleHandle");
    if (state == "True") {
        element.style.backgroundColor = "#D43E3E";
        child.style.removeProperty("left");
        child.style.right = "4px";
        //element.setAttribute("data-stat", "False");
        document.querySelector("#AvaliableForm").querySelector("input[type='hidden']").value = "False";
        console.log(document.forms[0]);
        document.forms[0].submit();
    }
    else {
        element.style.backgroundColor = "#43AF5C";
        child.style.removeProperty("right");
        child.style.left = "4px";
        //element.setAttribute("data-stat", "True");
        document.querySelector("#AvaliableForm").querySelector("input").value = "True";
        console.log(document.forms[0]);
        document.forms[0].submit();
    }
}
function LabAvailablewithout(element) {
    const state = element.getAttribute("data-stat");
    const child = element.querySelector(".toggleHandle");
    if (state == "False") {
        element.style.backgroundColor = "#D43E3E";
        child.style.removeProperty("left");
        child.style.right = "4px";
    }
    else {
        element.style.backgroundColor = "#43AF5C";
        child.style.removeProperty("right");
        child.style.left = "4px";
    }
}
window.addEventListener('DOMContentLoaded', function () {
    const element = document.getElementById('ToggleBtn');
    if (element != null) {
        LabAvailablewithout(element);
    }
});
/*function LabAvailableForm(element) {
    element.preventDefault();
    const btn = element.parentElement.querySelector("button");
    LabAvailable(btn);
}*/