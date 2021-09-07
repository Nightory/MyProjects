package com.example.search_item2;

import android.content.pm.ActivityInfo;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;
import androidx.appcompat.app.AppCompatActivity;
import java.io.FileInputStream;
import java.io.IOException;

public class AchievementsActivity extends AppCompatActivity {
    private final static String FILE_NAME = "content.txt";
    ImageView Image[] = new ImageView[5];
    TextView Text[] = new TextView[5];
    int Image_ID[] = new int[]{R.id.imageView,R.id.imageView2,R.id.imageView3,R.id.imageView4,R.id.imageView5};
    int Text_ID[] = new int[]{R.id.textView2,R.id.textView3,R.id.textView5,R.id.textView7,R.id.textView9};
    int unlock_ID[] = new int[]{R.drawable.achiv1,R.drawable.achiv2,R.drawable.achiv3,R.drawable.achiv4,R.drawable.achiv5};
    Button back;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        onWindowFocusChanged(true);
        setContentView(R.layout.activity_achievements);
        getSupportActionBar().hide();
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
        for(int i=0;i<5;i++){
            Image[i]=(ImageView) findViewById(Image_ID[i]);
            Text[i]=(TextView) findViewById(Text_ID[i]);
        }
        back = (Button) findViewById(R.id.button);
        back.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                AchievementsActivity.this.finish();
            }
        });
        Readfile();
    }
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
    public void Readfile(){
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
            for(int i=0;i<5;i++) {
                if(words[i].equals("true")){
                    Image[i].setImageResource(unlock_ID[i]);
                    Text[i].setText("Открыто");
                }
            }
        }
        catch(IOException ex) {

            Toast.makeText(this, ex.getMessage(), Toast.LENGTH_SHORT).show();
        }
    }
}