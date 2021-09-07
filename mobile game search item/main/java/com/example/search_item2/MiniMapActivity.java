package com.example.search_item2;

import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;

public class MiniMapActivity extends AppCompatActivity {
    ImageView level[];
    int level_ID[] = new int[]{R.id.level1,R.id.level2,R.id.level3};
    int LevelNum;
    Button back;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        onWindowFocusChanged(true);
        setContentView(R.layout.activity_mini_map);
        getSupportActionBar().hide();
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
        Bundle arguments = getIntent().getExtras();
        LevelNum = arguments.getInt("level");
        level = new ImageView[LevelNum];
        for(int i = 0; i < LevelNum; i++){
            level[i] = (ImageView) findViewById(level_ID[i]);
        }
        back = (Button) findViewById(R.id.back);
        back.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                MiniMapActivity.this.finish();
            }
        });
        level[0].setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View view, MotionEvent motionEvent) {
                Intent i=new Intent(MiniMapActivity.this,PlayActivity1.class);
                i.putExtra("level", 1);
                startActivity(i);
                return false;
            }
        });
        level[1].setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View view, MotionEvent motionEvent) {
                Intent i=new Intent(MiniMapActivity.this,PlayActivity1.class);
                i.putExtra("level", 2);
                startActivity(i);
                return false;
            }
        });
        level[2].setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View view, MotionEvent motionEvent) {
                Intent i=new Intent(MiniMapActivity.this,PlayActivity1.class);
                i.putExtra("level", 3);
                startActivity(i);
                return false;
            }
        });
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
}