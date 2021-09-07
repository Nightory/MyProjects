package com.example.search_item2;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.pm.ActivityInfo;
import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.constraintlayout.widget.ConstraintLayout;
import android.os.CountDownTimer;
import android.os.Handler;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Random;


public class PlayActivity1 extends AppCompatActivity  implements View.OnTouchListener {
    String[][] ItemName1 = {{"Символ Огня", "Горлянка", "Повязка", "Кастет", "Кунай", "Акацуки", "Метка", "Гунбай", "Кольцо", "Маска", "Ворон", "Печать", "Риннеган", "Учиха", "Свиток"},
            {"Фума сюрикен", "Меч забудзы", "Акацуки", "Учиха", "Самихада", "Шляпа", "Сюрикен", "Шаринган", "Кольцо", "Кунай", "Маска", "Свиток", "Символ огня", "Кастет", "Веер"},
            {"Кольцо", "Акамару", "Рамен", "Йена", "Повязка", "Кунай", "Веер", "Семь", "Коса", "Лягушка", "Горлянка", "Риннеган", "Фума сюрикен", "Учиха", "Ворон"}};
    Thread t,t2;
    HashSet<Integer> Number = new HashSet<Integer>();
    int WIN =0;
    int Image_Id1[][] =new int[][]{{R.id.imageView9,R.id.imageView10,R.id.imageView11,R.id.imageView12,R.id.imageView13,R.id.imageView14,R.id.imageView15,R.id.imageView16,
            R.id.imageView17,R.id.imageView18,R.id.imageView19,R.id.imageView20, R.id.imageView21,R.id.imageView22,R.id.imageView23},
            {R.id.imageView24,R.id.imageView25,R.id.imageView26,R.id.imageView27,R.id.imageView28,R.id.imageView29,R.id.imageView30,R.id.imageView31,
                    R.id.imageView32,R.id.imageView33,R.id.imageView34,R.id.imageView35, R.id.imageView36,R.id.imageView37,R.id.imageView38},
            {R.id.imageView39,R.id.imageView40,R.id.imageView41,R.id.imageView42,R.id.imageView43,R.id.imageView44,R.id.imageView45,R.id.imageView46,
                    R.id.imageView47,R.id.imageView48,R.id.imageView49,R.id.imageView50, R.id.imageView51,R.id.imageView52,R.id.imageView53}};
    int Text_Id1[] =new int[]{R.id.Item1,R.id.Item2,R.id.Item3,R.id.Item4,R.id.Item5,R.id.Item6,R.id.Item7,R.id.Item8,R.id.Item9,R.id.Item10};
    int[] item_number = new int[10];
    int Level, Missclick=0;
    int Background[]= new int []{R.drawable.fon1,R.drawable.fon2,R.drawable.fon3};
    public TextView timer, hints,menu;
    ConstraintLayout L[]=new ConstraintLayout[3];
    CountDownTimer cTimer = null,cTimer2 = null;
    TextView[] item_picture = new TextView[15];
    ImageView[] Image = new ImageView[15];
    Button butmenu, buthint;
    private final static String FILE_NAME = "content.txt";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        onWindowFocusChanged(true);
        Bundle arguments = getIntent().getExtras();
        Level = arguments.getInt("level")-1;
        setContentView(R.layout.activity_play1);
        getSupportActionBar().hide();
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE); //фиксация ориентации телефона
        L[0] = (ConstraintLayout) findViewById(R.id.Layout1);
        L[1] = (ConstraintLayout) findViewById(R.id.Layout2);
        L[2] = (ConstraintLayout) findViewById(R.id.Layout3);
        for(int i=0;i<L.length;i++){
            L[i].setVisibility(View.INVISIBLE);
            if(i==Level){
                L[i].setVisibility(View.VISIBLE);
                L[i].setOnTouchListener(this::onTouch);
            }
        }
        findViewById(R.id.Layout0).setBackgroundResource(Background[Level]);
        begining(); //Инициализация текстовых полей
        initialize_image(); //Инициализация картинок
        //Инициализация кнопок и полей которые будут на любом уровне
        timer = (TextView) findViewById(R.id.timer);
        menu = (TextView) findViewById(R.id.textViewMenu);
        hints = (TextView) findViewById(R.id.textViewHint);
        timer.setVisibility(View.VISIBLE);
        hints.setVisibility(View.VISIBLE);
        menu.setVisibility(View.VISIBLE);
        runThread();
        //переменные для кнопок
        butmenu = (Button) findViewById(R.id.buttonmenu);
        buthint = (Button) findViewById(R.id.buttonhint);
        butmenu.setVisibility(View.VISIBLE);
        buthint.setVisibility(View.VISIBLE);
        //переменные для изображений
        CreateList();//генерация спска рандомных предметов
        //Обработчик кнопки меню
        butmenu.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                AlertDialog.Builder dlgAlert  = new AlertDialog.Builder(PlayActivity1.this);
                dlgAlert.setMessage("Вы увеерены что хотите выйти? Вам будет засчитан проигрыш уровня");
                dlgAlert.setTitle("Предупреждение");
                dlgAlert.setPositiveButton("Да",
                        new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int which) {
                                PlayActivity1.this.finish();
                            }
                        });
                dlgAlert.setNegativeButton("Нет", null);
                dlgAlert.setCancelable(true);
                dlgAlert.create().show();
            }
        });
        //Обрабочик кнопки подсказка
        buthint.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                runThreadHint();
                buthint.setEnabled(false);
            }
        });
    }
    //Установливаем полноэкранный режим
    public void onWindowFocusChanged(boolean hasFocus) {
        super.onWindowFocusChanged(hasFocus);
        if (hasFocus) {
            getWindow().getDecorView().setSystemUiVisibility(
                    View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                            | View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                            | View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                            | View.SYSTEM_UI_FLAG_HIDE_NAVIGATION
                            | View.SYSTEM_UI_FLAG_FULLSCREEN
                            | View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY);
        }
    }
    //генерация спска рандомных предметов
    public void CreateList() {
        Random r = new Random();
        while(Number.size()<10){//В этот списое нельзя вносить одинаковые числа, он их игнорирует
            Number.add(r.nextInt(15));
        }
        Iterator<Integer> item = Number.iterator();
        int n=0;
        while (item.hasNext()) { //Переписываем полученные числа в наш массив, так как из этого списка не очень приятно доставать
            item_number[n] = item.next();
            n++;
        }
        for(int i=0;i<10;i++){//Заполняем переменные текстом на основе полученных чисел
            item_picture[i].setText(ItemName1[Level][item_number[i]]);
        }
    }

    public void begining() {
        //Инициализируем текстовые поля
        for(int i=0;i<10;i++){
            item_picture[i]=(TextView) findViewById(Text_Id1[i]);
        }

    }
    public void initialize_image(){
        //Инициализируем картинки и делаем их кликабельными
        for(int i=0;i<15;i++){
            Image[i]=(ImageView) findViewById(Image_Id1[Level][i]);
            Image[i].setOnTouchListener(this::onTouch);
        }
    }

    public void delete_text(int i) {
        //Стирание конкретного элемента из списка
        item_picture[i].setText("");
    }

    public void runThread() {//Запуск потока с таймером для карты
        t = new Thread(new Runnable() {
            public void run() {
                runOnUiThread(runn1);
            }
        });
        t.start();
    }
    public void runThreadHint() {//Запуск потока с таймером для подсказки
        t2 = new Thread(new Runnable() {
            public void run() {
                runOnUiThread(runn2);
            }
        });
        t2.start();
    }

    Runnable runn1 = new Runnable() { //Поток с таймером для игры
        public void run() {
            cTimer  = new CountDownTimer(60000, 1000) {

            public void onTick(long millisUntilFinished) {
                int times = Integer.parseInt(timer.getText()+"")-1;
                if(times-Missclick<=0){
                    cTimer.cancel();
                    AlertDialog.Builder dlgAlert  = new AlertDialog.Builder(PlayActivity1.this);
                    dlgAlert.setMessage("Неудача, вы не успели найти предметы за отведенное время");
                    dlgAlert.setTitle("Вы проиграли");
                    dlgAlert.setPositiveButton("Ок",
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int which) {
                                    PlayActivity1.this.finish();
                                }
                            });
                    dlgAlert.setCancelable(true);
                    dlgAlert.create().show();
                    timer.setText("0");
                }
                timer.setText((times-Missclick) + "");
                Missclick=0;
                if(WIN==10) {//Проверяем каждую секудну количество найденных предметов если нашли все останавдиваем таймер и выводим соответствующее окно
                    cTimer.cancel();
                    AlertDialog.Builder dlgAlert = new AlertDialog.Builder(PlayActivity1.this);
                    dlgAlert.setMessage("Поздравляем, вы успешно прошли уровень");
                    dlgAlert.setTitle("Вы выиграли");
                    SaveInFile();
                    dlgAlert.setPositiveButton("Ок",
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int which) {
                                    PlayActivity1.this.finish();
                                }
                            });
                    dlgAlert.setCancelable(true);
                    dlgAlert.create().show();
                }
            }

            public void onFinish() {//Время вышло выводим окно с проигрышем и закрываем уровень
                AlertDialog.Builder dlgAlert  = new AlertDialog.Builder(PlayActivity1.this);
                dlgAlert.setMessage("Неудача, вы не успели найти предметы за отведенное время");
                dlgAlert.setTitle("Вы проиграли");
                dlgAlert.setPositiveButton("Ок",
                        new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int which) {
                                PlayActivity1.this.finish();
                            }
                        });
                dlgAlert.setCancelable(true);
                dlgAlert.create().show();
                timer.setText("0");
            }
        };
        cTimer.start();
        }
    };
    Runnable runn2 = new Runnable() {//Поток с таймером для подсказки
        @Override
        public void run() {//Проверяем список предметов и берем самый первый не найденный
            int num = -1;
            for(int i = 0; i < 10; i++){
                if (item_number[i]!=-1){
                    num = item_number[i];
                    break;
                }
            }
            if(num !=-1) {//Если такой есть то делаем выбранный предмет моргающим
                    Image[num].setVisibility(View.GONE);
                    Handler handler = new Handler();
                int finalNum = num;
                handler.postDelayed(new Runnable() {
                        @Override
                        public void run() {
                            Image[finalNum].setVisibility(View.VISIBLE);
                        }
                    }, 500);

            }
            cTimer2  = new CountDownTimer(20000, 1000) {//Как только кончится таймер делаем кнопку активной

                public void onTick(long millisUntilFinished) {
                    hints.setText((millisUntilFinished / 1000) + "");
                }
                public void onFinish() {
                    buthint.setEnabled(true);
                    hints.setText("Hint");
                }
            };
            cTimer2.start();
        }
    };
    @Override
    public boolean onTouch(View view, MotionEvent motionEvent) {//Обработчик нажатий на картинки
        int ID = view.getId();
        for (int i = 0; i < 15; i++) {
            if (ID == Image_Id1[Level][i]) {
                for (int j = 0; j < item_number.length; j++) {
                    if (item_number[j] == i) {
                        Image[i].setVisibility(View.GONE);
                        delete_text(j);
                        item_number[j]=-1;
                        WIN++;
                        Missclick-=2;
                        return false;
                    }
                }
            }
        }
        Missclick+=2;
        return false;
    }
    public void SaveInFile(){
        FileOutputStream fos = null;
        FileInputStream fin = null;
        try {
            fin = openFileInput(FILE_NAME);
            byte[] bytes = new byte[fin.available()];
            fin.read(bytes);
            String text = new String(bytes);
            String[] words = text.split(" ");
            for (String word : words) {
                System.out.println(word);
            }
            String text2 = timer.getText().toString();
            int time =Integer.parseInt (text2);
            fos = openFileOutput(FILE_NAME, MODE_PRIVATE);
            for(int i = 0; i < 5; i++) {
                if(words[i].equals("true")){
                    fos.write("true ".getBytes());
                }
                else{
                    if(i==Level){
                        fos.write("true ".getBytes());
                        words[i]="true";
                    }
                    else if (i == 3 && time > 30){
                        fos.write("true ".getBytes());
                        words[i]="true";
                    }
                    else fos.write("false ".getBytes());
                }
                if(words[0].equals("true") && words[1].equals("true") && words[2].equals("true") && words[3].equals("true"))  words[4] ="true";
            }
        }
        catch(IOException ex) {

            Toast.makeText(this, ex.getMessage(), Toast.LENGTH_SHORT).show();
        }
    }
}

