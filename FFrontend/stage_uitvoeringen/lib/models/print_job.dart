class PrintJob {
  final int printerId;
  final int documentId;

  PrintJob({required this.printerId, required this.documentId});

  factory PrintJob.fromJson(Map<String, dynamic> json) {
    return PrintJob(
      printerId: json['printerId'],
      documentId: json['documentId'],
    );
  }

  Map<String, dynamic> toJson() => {
    'printerId': printerId,
    'documentId': documentId,
  };
}
