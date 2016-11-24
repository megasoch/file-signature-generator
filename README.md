# file-signature-generator

Алгоритм работы программы

Используется 2 байтовых буфера по 32Мб по умолчанию(ByteBuffer.cs), первый - для чтения файла с диска, второй - для обработки прочитанных данных.

В главном потоке программы происходит чтение файла в буфер, когда один буфер заполняется, создается поток для обработки заполненного буфера, главный поток продолжает чтение во второй буфер.

В потоке обработки создаются дополнительные потоки для параллельного подсчета хэша. 
Количество потоков считающих хэш, зависит от числа ядер процессора.
Для последовательного вывода хэшей используется Dictionary<номер_блока, хэш>.

Файл читается(FileByteInputer.cs) блоками заданного размера(64Кб).

Предполагается что максимальный размер обрабатываемого блока ограничен, соответственно меняется размер буфера.