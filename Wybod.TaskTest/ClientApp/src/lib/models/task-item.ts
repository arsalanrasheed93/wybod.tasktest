export interface TaskItem {
  id: string
  title: string
  description: string
  isCompleted: boolean
  createdAt: string
  completedAt?: string| null
  priority?: string | null
  isActive?: boolean | true
}