<script src="http://code.jquery.com/jquery-2.0.2.min.js"></script>
    <script>
        $(document).ready(function(){
            //Скрыть PopUp при загрузке страницы    
            PopUpHide();
        });
    //Функция отображения PopUp
    function PopUpShow(){
            $("#popup1").show();
        }
    //Функция скрытия PopUp
    function PopUpHide(){
            $("#popup1").hide();
        }
</script>