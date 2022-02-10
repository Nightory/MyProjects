#include <iostream>
#include "Queue.h"
//#include "QueueList.h"

int getRandomNumber(int min, int max) 
{
    static const double fraction = 1.0 / (static_cast<double>(RAND_MAX) + 1.0);
    return static_cast<int>(rand() * fraction * (max - min + 1) + min);
}

struct Detail {
    char id[4];
    int time;
};

Detail* mas;
Detail d;

int main()
{
    setlocale(LC_ALL, "rus");
    Queue<Detail>* q;
    q = new Queue<Detail>();
    int key;
    int stage=-1;
    bool flag = true;
    while (flag)
    {
        std::cout << "1.Поставить деталь в очередь" << std::endl;
        std::cout << "2.Перейти к следующему шагу" << std::endl;
        std::cout << "3.Снять деталь с обработки" << std::endl;
        std::cout << "4.Вывод списка деталей" << std::endl;
        std::cout << "5.Сброс/Инициализация" << std::endl;
        std::cout << "6.Выход" << std::endl;
        std::cin >> key;
        switch (key) 
        {
            case 1:
            {
                Detail D;
                for (int i = 0; i < 4; i++) {
                    D.id[i] = '0' + getRandomNumber(0,9);
                }
                D.time = getRandomNumber(1, 5);
                bool check=q->QueueAdd(D);
                if(check) std::cout << "Очередь переполнена" << std::endl;
            }
            break;

            case 2:
            {
                if (stage == -1) {
                    d = q->Peek();
                    stage = d.time - 1;
                }
                else 
                {
                    stage--;
                }
                if (stage == 0)
                {
                    d = q->Dequeue();
                    if(d.time!=NULL)
                    std::cout << "Деталь с кодом:" << d.id[0] << d.id[1] << d.id[2] << d.id[3] << "     Готова" << std::endl;
                    stage = -1;
                }
            }
            break;

            case 3:
            {
                d = q->Dequeue();
                if(stage!=-1)
                    std::cout << "Деталь с кодом:" << d.id[0] << d.id[1] << d.id[2] << d.id[3] << "     Оставшиеся шаги:" << stage << "\nСнята с произодства" << std::endl;
                else
                {
                    if (d.time != NULL)
                    {
                        std::cout << "Деталь с кодом:" << d.id[0] << d.id[1] << d.id[2] << d.id[3] << "     Оставшиеся шаги:" << d.time << "\nСнята с произодства" << std::endl;
                        d = q->Peek();
                        stage = d.time;
                    }
                }
            }
            break;

            case 4:
            {
                int size;
                mas = q->ViewDetail(size);
                for (int i = 0; i < size; i++)
                {
                    if(i==0 && stage!=-1) std::cout << "Код детали:" << mas[i].id[0] << mas[i].id[1] << mas[i].id[2] << mas[i].id[3] << "   Количество шагов:" << stage << std::endl;
                    else std::cout << "Код детали:" << mas[i].id[0] << mas[i].id[1] << mas[i].id[2] << mas[i].id[3] << "   Количество шагов:" << mas[i].time << std::endl;
                }
                delete[] mas;
            }
            break;

            case 5:
            {
                q = new Queue<Detail>();
            }

            break;
            case 6:
            {
                flag = false;
            }
            break;

        }
    }
}

