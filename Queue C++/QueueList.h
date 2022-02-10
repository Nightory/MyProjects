#pragma once

template <class Temp>
struct QueueList
{
    QueueList* next;
    Temp data;
};

template <class Temp>
class Queue {
private:
    QueueList<Temp> *head, *tail;
    int countEl;
public:
    Queue();
    ~Queue();
    bool Empty();
    bool Overflow();
    bool QueueAdd(Temp T);
    Temp Dequeue();
    Temp* ViewDetail(int& size);
    Temp Peek();
};

template <class Temp>
Queue<Temp>::Queue()
{
    head = NULL;
    tail = NULL;
    countEl = 0;
}

template <class Temp>
bool Queue<Temp>::QueueAdd(Temp T)
{
    if (Overflow())
    {
        return true;
    }
    else
    {
        QueueList<Temp>* tmp = new QueueList<Temp>();
        tmp->data = T;
        tmp->next = NULL;
        if (!Empty())
        {
            tail->next = tmp;
        }
        else
        {
            head = tmp;
        }
        tail = tmp;
        countEl++;
        return false;
    }
}

template <class Temp>
Temp Queue<Temp>::Dequeue()
{
    Temp T = Temp();
    if (!Empty())
    {
        T = head->data;
        QueueList<Temp>* deleted = head;
        head = head->next;
        delete deleted;
        countEl--;
        return T;
    }
    else
    {
        std::cout << "Очередь пуста!" << std::endl;
        return T;
    }

}

template <class Temp>
bool Queue<Temp>::Empty()
{
    if (head == NULL) return true;
    else return false;
}

template <class Temp>
bool Queue<Temp>::Overflow()
{
    return false;
}

template <class Temp>
Temp* Queue<Temp>::ViewDetail(int& size)
{
    if (Empty())
    {
        size = 0;
        return NULL;
    }
    size = countEl;
    Temp* arr = new Temp[countEl];
    int j = 0;
    QueueList<Temp>* currentPtr = head;
    while (currentPtr != NULL)
    {
        arr[j] = currentPtr->data;
        currentPtr = currentPtr->next;
        j++;
    }
    return arr;
}

template <class Temp>
Temp Queue<Temp>::Peek()
{
    Temp T = Temp();
    if (!Empty())
    {
        T = head->data;
        return T;
    }
    else
    {
        std::cout << "Очередь пуста!" << std::endl;
        return T;
    }

}
