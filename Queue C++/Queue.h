#pragma once

template <class Temp>
class Queue {
private:
    int head, tail, maxEl = 6;
    Temp* array;
public:
    Queue();
    ~Queue();
    bool Empty();
    bool Overflow();
    bool QueueAdd(Temp T);
    Temp Dequeue();
    Temp* ViewDetail(int &size);
    Temp Peek();
};

template <class Temp>
Queue<Temp>::Queue()
{
    head = 0;
    tail = 0;
    array = new Temp[maxEl];
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
        array[tail] = T;
        tail = (tail + 1) % maxEl;
        return false;
    }
}

template <class Temp>
Temp Queue<Temp>::Dequeue()
{
    Temp T = Temp();
    if (!Empty())
    {
        T = array[head];
        head = (head + 1) % maxEl;
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
    if (head == tail) return true;
    else return false;
}

template <class Temp>
bool Queue<Temp>::Overflow()
{
    if (head == ((tail+1) % maxEl)) return true;
    else return false;
}

template <class Temp>
Temp* Queue<Temp>::ViewDetail(int &size)
{
    if (Empty())
    {
        size = 0; 
        return NULL;
    }
    size = head < tail ? tail - head : maxEl - head + tail;
    Temp* arr = new Temp[size];
    int j = head;
    for (int i = 0; i < size; i++)
    {
        arr[i] = array[j++ % maxEl];
    }
    return arr;
}

template <class Temp>
Temp Queue<Temp>::Peek()
{
    Temp T = Temp();
    if (!Empty())
    {
        T = array[head];
        return T;
    }
    else
    {
        std::cout << "Очередь пуста!" << std::endl;
        return T;
    }

}
